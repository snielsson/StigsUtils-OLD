// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System.Globalization;
namespace StigsUtils.Services.CsvServices;

public interface ICsvService {
	byte[] CreateCsvFileBytes(IEnumerable<dynamic> data, CultureInfo? culture = null);
}