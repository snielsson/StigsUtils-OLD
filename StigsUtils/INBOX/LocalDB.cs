// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.

// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using Dapper;
// using Microsoft.Data.SqlClient;
// using Microsoft.Extensions.Logging;
// using RegTech.Shared.Common.Extensions;
//
// namespace RegTech.Shared.SqlServer
// {
//     /// <summary>
//     ///     Provides easy to use API for the local version of SqlServer called LocalDb. This enables full test coverage of
//     ///     database schema and a way to start from an empty database and then add test data as needed for each test to ensure
//     ///     good test coverage.
//     /// </summary>
//     public static class LocalDb
//     {
//         public const string LocalDbDataSource = @"(LocalDB)\MSSQLLocalDB";
//         public static string DefaultDataDirectory { get; } = @"C:\TEMP\LocalDb";
//
//         public static SqlConnection MasterConnection => Connect("Master");
//
//         public static string CreateLocalDbConnectionString(string databaseName) =>
//             @$"Data Source={LocalDbDataSource};Connect Timeout=3;Integrated Security=true;Initial Catalog={databaseName};";
//
//         public static SqlConnection Connect(string databaseName)
//         {
//             var connectionString = CreateLocalDbConnectionString(databaseName);
//             var connection = new SqlConnection(connectionString);
//             connection.Open();
//             return connection;
//         }
//
//         public static void Reset(string connectionString, ILogger? log = null, string? dataFileDirectory = null, bool force = false)
//         {
//             InternalReset(connectionString, null, log, dataFileDirectory, force);
//         }
//
//         public static void Reset(string connectionString, IEnumerable<string> sqlScripts, ILogger? log = null, string? dataFileDirectory = null, bool force = false)
//         {
//             InternalReset(connectionString, sqlScripts, log, dataFileDirectory, force);
//         }
//
//         private static void InternalReset(string connectionString, IEnumerable<string>? sqlScripts, ILogger? log = null, string? dataFileDirectory = null, bool force = false)
//         {
//             var sqlConnectionString = new SqlConnectionStringBuilder(connectionString);
//             if (!sqlConnectionString.DataSource.StartsWith("(LocalDB)"))
//             {
//                 var msg =$"Reset cannot be called for non-local db server.\nConnection string:\n{connectionString}";
//                 log.LogError(msg);
//                 throw new InvalidOperationException(msg);
//             }
//
//             log.LogInformation($"Dropping database {sqlConnectionString}");
//             DropDatabase(sqlConnectionString.InitialCatalog, force);
//             log.LogInformation($"Creating database {sqlConnectionString} using data file directory {dataFileDirectory}.");
//             CreateDatabase(sqlConnectionString.InitialCatalog, dataFileDirectory);
//             using var masterConnection = MasterConnection;
//             if (sqlScripts != null)
//             {
//                 var sqlScriptsArray = sqlScripts.ToArray();
//                 int count = 0;
//                 foreach (var script in sqlScriptsArray)
//                 {
//                     log.LogTrace($@"
//
// ==============================================
// Executing SQL script {++count} of {sqlScriptsArray.Length} on {sqlConnectionString} to initialize database. 
// Contents of script:
// ----------------------------------------------
// {script}");
//                     masterConnection.ExecuteSqlScript(script);
//                 }
//             }
//             log.LogTrace($@"
// ===============================================
//
// Done creating database {sqlConnectionString} in data file directory {dataFileDirectory}.
//
// ");
//         }
//
//         public static bool CheckDatabaseExists(string databaseName)
//         {
//             using SqlConnection db = Connect("master");
//             var result = db.ExecuteScalar<object>($"SELECT db_id('{databaseName}')");
//             return result != null;
//         }
//
//         public static int CreateDatabase(string databaseName, string? dataFileDirectory = null, LocalDbCreationOptions? options = null)
//         {
//             options ??= LocalDbCreationOptions.GetDefaultValues();
//
//             var directory = Path.GetFullPath(dataFileDirectory ?? DefaultDataDirectory);
//             if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
//             var filepathWithoutExtension = Path.Combine(directory, databaseName);
//             using SqlConnection db = Connect("master");
//             var sql = $@"
// 				CREATE DATABASE [{databaseName}] 
// 				ON PRIMARY(NAME=[{databaseName}],FILENAME='{filepathWithoutExtension + ".mdf"}',
// 				SIZE = {options.Size}MB, MAXSIZE = {options.MaxSize}MB, FILEGROWTH = {options.Growth}MB)
// 				LOG ON (NAME={databaseName + "_log"},FILENAME='{filepathWithoutExtension + ".ldf"}')";
//             return db.Execute(sql);
//         }
//
//         public static int ExecuteSqlAsMaster(string sql)
//         {
//             using SqlConnection db = Connect("master");
//             var result = db.Execute(sql);
//             return result;
//         }
//
//         public static void EnsureDatabaseExists(string dbName)
//         {
//             if (DbExists(dbName)) return;
//             CreateDatabase(dbName);
//         }
//
//         public static bool DbExists(string name)
//         {
//             using SqlConnection db = Connect("master");
//             var result = db.ExecuteScalar($"SELECT DB_ID('{name.Replace("'", "")}')");
//             return result != null;
//         }
//
//         public static void DropDatabase(string databaseName, bool force)
//         {
//             using SqlConnection db = Connect("master");
//             if (DbExists(databaseName))
//             {
//                 if (force) db.Execute($@"ALTER DATABASE {databaseName} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;");
//                 db.Execute($@"DROP DATABASE {databaseName}");
//             }
//         }
//
//         /*
//         public static string[] GetDatabaseScripts(string connectionString) => GetDatabaseScripts(new SqlConnectionStringBuilder(connectionString));
//
//         public static string[] GetDatabaseScripts(SqlConnectionStringBuilder connectionStringBuilder)
//         {
//             var sqlServer = new Server(connectionStringBuilder.DataSource);
//             Database db = sqlServer.Databases[connectionStringBuilder.InitialCatalog];
//             var scripter = new Scripter(sqlServer)
//             {
//                 Options = new ScriptingOptions
//                 {
//                     //Options as used by SMSS when using the Generate Scripts task for an entire database.
//                     AnsiPadding = false,
//                     AppendToFile = false
//                     //	,IncludeIfNotExists = true
//                     ,
//                     ContinueScriptingOnError = false, ConvertUserDefinedDataTypesToBaseType = false
//                     //,WithDependencies = true
//                     ,
//                     IncludeHeaders = true, IncludeDatabaseRoleMemberships = true, IncludeScriptingParametersHeader = false, SchemaQualify = true,
//                     ExtendedProperties = true, TargetServerVersion = SqlServerVersion.Version130, TargetDatabaseEngineEdition = DatabaseEngineEdition.Personal,
//                     TargetDatabaseEngineType = DatabaseEngineType.Standalone, DriAll = true, DriWithNoCheck = true, NoIdentities = false, ScriptOwner = true,
//                     ScriptSchema = true, Permissions = true
//                 }
//             };
//             StringCollection scripts = scripter.Script(new[] {db.Urn});
//             return null;//scripts.ToArray();
//         }
//
//         public static SqlConnection MakeEmptyLocalDbDatabaseCopy(string connectionString, string dbName = null)
//         {
//             var sqlConnection = new SqlConnection(connectionString);
//             var sourceServer = new Server(new ServerConnection(sqlConnection));
//             Database sourceDb = sourceServer.Databases[sqlConnection.Database];
//
//             var destinationServer = new Server(LocalDbDataSource);
//             destinationServer.ConnectionContext.LoginSecure = true;
//             destinationServer.ConnectionContext.Connect();
//             dbName = "RegulatoryReporting";
//             if(destinationServer.Databases.Contains(dbName)) destinationServer.KillDatabase(dbName);
//             var destinationDb = new Database(destinationServer,dbName);
//             destinationDb.Create();
//             var sql = File.ReadAllText("C:/TEMP/LocalDB/migration1.sql");
//             destinationDb.ExecuteNonQuery(sql);
//             //var transferService = new Transfer(sourceDb)
//             //{
//             //	DestinationServer = destinationServer.Name, 
//             //	DestinationDatabase = destinationDb.Name,
//             //	CopyAllObjects = true, 
//             //	CopyData = false, 
//             //	Options = new ScriptingOptions
//             //	{
//             //		//ContinueScriptingOnError = true, // Current database contains errors - should be fixed.
//             //		WithDependencies = true,
//             //		//DriChecks = false,
//             //		DriWithNoCheck = true, 
//             //		//DriAll = true
//             //	}
//             //};
//
//             //trsfrDB.CopyAllSchemas = true;
//             ////Copy all user defined data types from source to destination
//             //trsfrDB.CopyAllUserDefinedDataTypes = true;
//             ////Copy all tables from source to destination
//             //trsfrDB.CopyAllTables = true;
//             ////Copy data of all source tables to destination tables
//             ////It actually generates INSERT statement for destination
//             ////Copy all stored procedure from source to destination
//             //trsfrDB.CopyAllStoredProcedures = true;
//             ////specify the destination server name
//             //var start = DateTime.UtcNow;
//             //transferService.TransferData();
//             //var elapsed = DateTime.UtcNow - start;
//
//             var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
//             dbName ??= connectionStringBuilder.InitialCatalog;
//             var dataDirectory = "c:/TEMP/LocalDb";
//             var mdfFilePath = Path.GetFullPath(Path.Combine(dataDirectory, dbName + ".mdf"));
//             var ldfFilePath = Path.GetFullPath(Path.Combine(dataDirectory, dbName + "_log.ldf"));
//             var scripts = GetDatabaseScripts(connectionStringBuilder);
//             scripts[0] = scripts[0]
//                     //Remove size specifications from source database and use default values.
//                     .RegexReplace(@" , SIZE = [^)]*", "")
//                     //Set database name
//                     .RegexReplace(@"(CREATE DATABASE \[)([^]]+)(])", $"$1{dbName}$3")
//                     .RegexReplace(@"( NAME = N')(.+?)(_log)?(')", $"$1{dbName}$3$4")
//                     //Set database filepaths
//                     .RegexReplace(@"(FILENAME = N')(.*?\.mdf)(')", $"$1{mdfFilePath}$3")
//                     .RegexReplace(@"(FILENAME = N')(.*?_log\.ldf)(')", $"$1{ldfFilePath}$3")
//                 ;
//
//             File.WriteAllLines("c:/TEMP/LocalDb/scripts_dump.sql", scripts);
//
//             DropDatabase(dbName);
//             SqlConnection db = Connect("master");
//             db.ExecuteSql(scripts);
//             return db;
//         }
//             */
//     }
// }
