// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.
using System.Text.RegularExpressions;
namespace StigsUtils.Extensions;

public static class RegexExtensions {
	public static string RegexReplace(this string @this, string pattern, string replacement) => @this.RegexReplace(new Regex(pattern), replacement);
	public static string RegexReplace(this string @this, Regex regex, string replacement) => regex.Replace(@this, replacement);
}