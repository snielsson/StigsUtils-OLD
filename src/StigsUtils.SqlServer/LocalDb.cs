// Copyright © 2021 Stig Schmidt Nielsson. This file is open source and distributed under the MIT License, see LICENSE file.

using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using StigsUtils.SqlServer.Extensions;

namespace StigsUtils.SqlServer {
	public static class LocalDb {
		public const string LocalDbDataSource = @"(LocalDB)\MSSQLLocalDB";
		public static string DefaultDataDirectory { get; set; } = @"C:\TEMP\LocalDb";
		public static string CreateLocalDbConnectionString(string databaseName) => @$"Data Source={LocalDbDataSource};Connect Timeout=3;Integrated Security=true;Initial Catalog={databaseName};";
        
		public static SqlConnection Open(string databaseName) {
			var connectionString = CreateLocalDbConnectionString(databaseName);
			var connection = new SqlConnection(connectionString);
			connection.Open();
			return connection;
		}
        
		public static void Reset(string databaseName, IEnumerable<string> sqlScriptContent) {
			using var db = LocalDb.Open("master");
			db.DropDatabaseIfExists(databaseName).CreateDatabase(databaseName, LocalDb.DefaultDataDirectory);
			foreach (var sql in sqlScriptContent) {
				db.ExecuteSqlScriptContent(sql);
			}
		}        
	}
}