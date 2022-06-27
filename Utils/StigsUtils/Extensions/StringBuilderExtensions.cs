// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System.Text;
namespace StigsUtils.Extensions;

public static class StringBuilderExtensions {

	public static bool EndsWith(this StringBuilder @this, string s) {
		if (s.IsNullOrEmpty()) return false;
		if (s.Length > @this.Length) return false;
		var pos = @this.Length;
		for (var i = s.Length - 1; i >= 0; i--) {
			pos--;
			if (pos == -1) return false;
			if (@this[pos] != s[i]) return false;
		}
		return true;
	}

	public static string Flush(this StringBuilder @this) {
		var result = @this.ToString();
		@this.Clear();
		return result;
	}

	public static StringBuilder FlushTo(this StringBuilder @this, TextWriter target) {
		target.Write(@this);
		return @this.Clear();
	}

	public static StringBuilder FlushTo(this StringBuilder @this, out string target) {
		target = @this.ToString();
		return @this.Clear();
	}

	public static char LastOrDefault(this StringBuilder @this) => @this.Length == 0 ? default : @this[^1];
}