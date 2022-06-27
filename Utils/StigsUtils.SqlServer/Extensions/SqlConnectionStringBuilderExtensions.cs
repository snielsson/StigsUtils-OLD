// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using Microsoft.Data.SqlClient;
namespace StigsUtils.SqlServer.Extensions;

public static class SqlConnectionStringBuilderExtensions {
	public const string LocalDbDataSource = @"(LocalDB)\MSSQLLocalDB";

	public static void ResetLocalDb(this SqlConnectionStringBuilder @this, IEnumerable<string> sqlScriptContent) {
		LocalDb.Reset(@this.InitialCatalog, sqlScriptContent);
	}

	public static SqlConnectionStringBuilder UseLocalDb(this SqlConnectionStringBuilder @this, string dbName) {
		@this.ConnectionString = @$"Data Source={LocalDbDataSource};Connect Timeout=3;Integrated Security=true;Initial Catalog={dbName};";
		return @this;
	}
}