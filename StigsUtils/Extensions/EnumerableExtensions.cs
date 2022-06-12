// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.
namespace StigsUtils.Extensions;

public static class EnumerableExtensions {
	public static IEnumerable<T> OrEmpty<T>(this IEnumerable<T>? @this) {
		if (@this == null) return Array.Empty<T>();
		return @this;
	}

	public static decimal Mean(this IEnumerable<decimal> @this) {
		long i = 0;
		decimal sum = 0;
		foreach (var x in @this) {
			sum += x;
			i++;
		}
		return sum / i;
	}
}