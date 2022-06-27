// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
namespace StigsUtils.Extensions;

public static class EnumerableExtensions {

	public static ICollection<T> AddTo<T>(this IEnumerable<T> @this, ICollection<T> target) {
		if (@this == null) throw new ArgumentNullException(nameof(@this));
		if (target == null) throw new ArgumentNullException(nameof(target));
		foreach (T item in @this) target.Add(item);
		return target;
	}

	[Obsolete("NOT TESTED")]
	public static void ForEach<T>(this IEnumerable<T> @this, Action<T> action) {
		if (@this == null) throw new ArgumentNullException(nameof(@this));
		foreach (T item in @this) action(item);
	}

	[Obsolete("NOT TESTED")]
	public static bool IsSorted<T>(this IEnumerable<T> @this, IComparer<T>? comparer = null, bool strictSorting = false) {
		if (@this == null) throw new ArgumentNullException(nameof(@this));
		if (strictSorting) return @this.IsSortedStrict(comparer);
		comparer ??= Comparer<T>.Default;
		using (IEnumerator<T> enumerator = @this.GetEnumerator()) {
			if (!enumerator.MoveNext()) return true;
			T prev = enumerator.Current;
			while (enumerator.MoveNext()) {
				if (comparer.Compare(prev, enumerator.Current) > 0) return false;
				prev = enumerator.Current;
			}
			return true;
		}
	}

	[Obsolete("NOT TESTED")]
	public static bool IsSortedStrict<T>(this IEnumerable<T> @this, IComparer<T>? comparer = null) {
		if (@this == null) throw new ArgumentNullException(nameof(@this));
		comparer ??= Comparer<T>.Default;
		using (IEnumerator<T> enumerator = @this.GetEnumerator()) {
			if (!enumerator.MoveNext()) return true;
			T prev = enumerator.Current;
			while (enumerator.MoveNext()) {
				if (comparer.Compare(prev, enumerator.Current) >= 0) return false;
				prev = enumerator.Current;
			}
			return true;
		}
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

	[Obsolete("NOT TESTED")]
	public static IEnumerable<T> OrEmpty<T>(this IEnumerable<T>? @this) {
		if (@this == null) return Array.Empty<T>();
		return @this;
	}

	public static T PickRandom<T>(this IEnumerable<T> @this, Random random, int? count = null) {
		var index = random.Next(0, count ?? @this.Count());
		return @this.Skip(index).First();
	}

	public static T PickRandom<T>(this IEnumerable<T> @this, int seed = 0, int? count = null) => PickRandom(@this, new Random(seed), count);
}