// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
namespace StigsUtils.SqlServer.Extensions;

public static class SqlConnectionExtensions {

	public static SqlConnection CreateDatabase(this SqlConnection @this, string databaseName, string dataFileDirectory, int sizeInMb = 64, int maxSizeInMb = 1024, int fileGrowthInMb = 8) {
		var directory = Path.GetFullPath(dataFileDirectory);
		if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
		var filepathWithoutExtension = Path.Combine(directory, databaseName);
		var sql = $@"
				CREATE DATABASE [{databaseName}] 
				ON PRIMARY(NAME=[{databaseName}],FILENAME='{filepathWithoutExtension + ".mdf"}',
				SIZE = {sizeInMb}MB, MAXSIZE = {maxSizeInMb}MB, FILEGROWTH = {fileGrowthInMb}MB)
				LOG ON (NAME={databaseName + "_log"},FILENAME='{filepathWithoutExtension + ".ldf"}')";
		@this.ExecuteNonQuery(sql);
		return @this;
	}

	public static bool DbExists(this SqlConnection @this, string name) {
		name = name.Replace("'", "");
		var result = @this.ExecuteScalar($"SELECT DB_ID('{name}')");
		var b = result is not DBNull;
		return b;
	}

	public static SqlConnection DropDatabase(this SqlConnection @this, string databaseName, bool force = false) {
		if (force) @this.ExecuteNonQuery($@"ALTER DATABASE {databaseName} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;");
		@this.ExecuteNonQuery($@"DROP DATABASE {databaseName}");
		return @this;
	}

	public static SqlConnection DropDatabaseIfExists(this SqlConnection @this, string databaseName, bool force = false) {
		if (@this.DbExists(databaseName)) @this.DropDatabase(databaseName, force);
		return @this;
	}

	public static int ExecuteNonQuery(this SqlConnection @this, string sql) {
		using SqlCommand cmd = new() {
			Connection = @this,
			CommandText = sql,
			CommandType = CommandType.Text
		};
		return cmd.ExecuteNonQuery();
	}

	public static object? ExecuteScalar(this SqlConnection @this, string sql) {
		using SqlCommand cmd = new() {
			Connection = @this,
			CommandText = sql,
			CommandType = CommandType.Text
		};
		return cmd.ExecuteScalar();
	}

	public static void ExecuteSqlScriptContent(this SqlConnection @this, string sql) {
		using var db = new SqlConnection(@this.ConnectionString);
		Server server = new(new ServerConnection(db));
		server.ConnectionContext.ExecuteNonQuery(sql);
	}
}