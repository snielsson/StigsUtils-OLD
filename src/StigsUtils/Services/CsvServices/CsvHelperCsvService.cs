// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System.Globalization;
using System.Text;
using CsvHelper;
namespace StigsUtils.Services.CsvServices;

/// <summary>
///   ICsvService implementation which uses CsvWriter from the CsvHelper package.
/// </summary>
public class CsvHelperCsvService : ICsvService {

	//TODO: Test CsvService.CreateCsvFileBytes.
	/// <summary>
	///   Serialize an enumeration of dynamics to an array of bytes.
	/// </summary>
	public byte[] CreateCsvFileBytes(IEnumerable<dynamic> data, CultureInfo? culture = null) {
		culture ??= CultureInfo.InvariantCulture;
		using var stringWriter = new StringWriter();
		using var csvWriter = new CsvWriter(stringWriter, culture);
		csvWriter.WriteRecords(data);
		return Encoding.UTF8.GetBytes(stringWriter.ToString());
	}
}