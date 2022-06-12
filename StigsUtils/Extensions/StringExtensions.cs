// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.
namespace StigsUtils.Extensions;

public static class StringExtensions {
	public static IEnumerable<long>? ParseLongs(this string @this, char sep = ',') {
		foreach (var x in @this.Split(',')) {
			if (!long.TryParse(x, out var parsed)) throw new ArgumentException($"Could not parse {x} as a long.");
			yield return parsed;
		}
	}

	public static bool TryParseLongs(this string @this, out List<long> result, char sep = ',') {
		var strings = @this.Split(',');
		result = new List<long>(strings.Length);
		foreach (var x in @this.Split(',')) {
			if (!long.TryParse(x, out var parsed)) return false;
			result.Add(parsed);
		}
		return true;
	}
}