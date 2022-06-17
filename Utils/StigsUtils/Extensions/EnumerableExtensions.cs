// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
namespace StigsUtils.Extensions;

public static class EnumerableExtensions {

	public static IEnumerable<(T, int)> Occurrences<T>(this IEnumerable<T> @this, int minValue = int.MinValue, int maxValue = int.MaxValue) =>
		@this.GroupBy(x => x).Select(x => (x.Key, x.Count())).Where(x => x.Item2 >= minValue && x.Item2 <= maxValue);

	public static IEnumerable<TU> ValidateDistinct<T, TU>(this IEnumerable<T> @this, Func<(T, int), TU> onValidationError) => @this.Occurrences(2).Select(onValidationError);

	public static IEnumerable<TU> ValidateWhiteList<T, TU>(this IEnumerable<T> @this, IEnumerable<T> whitelist, Func<T, TU> onValidationError) => @this.Except(whitelist).Distinct().Select(onValidationError);

	public static IEnumerable<TU> ValidateBlackList<T, TU>(this IEnumerable<T> @this, IEnumerable<T> blackList, Func<T, TU> onValidationError) => @this.Intersect(blackList).Distinct().Select(onValidationError);

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