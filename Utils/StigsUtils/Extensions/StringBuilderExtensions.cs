// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System.Text;
namespace StigsUtils.Extensions;

public static class StringBuilderExtensions {
	public static string Flush(this StringBuilder @this) {
		var result = @this.ToString();
		@this.Clear();
		return result;
	}
}