// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System.Collections;
namespace StigsUtils.Extensions;

public static class ArrayExtensions {
	
	public static IEnumerable ToEnumerable(this Array @this) {
		var enumerator = @this.GetEnumerator();
		while (enumerator.MoveNext()) yield return enumerator.Current;
	}
	public static IEnumerable<T> ToEnumerable<T>(this Array @this) {
		var enumerator = @this.GetEnumerator();
		while (enumerator.MoveNext()) yield return (T) enumerator.Current;
	}
	public static T[] ToArray<T>(this Array @this) {
		var result = new T[@this.LongLength];
		for (var i = 0; i < @this.LongLength; i++) {
			result[i] = (T)@this.GetValue(i)!;
		}
		return result;
	}
	public static object[] ToArray(this Array @this) {
		var result = new object[@this.LongLength];
		for (var i = 0; i < @this.LongLength; i++) {
			result[i] = @this.GetValue(i)!;
		}
		return result;
	}
	
	public static T[] AsArray<T>(this T @this) => new[] {@this};

	/// <summary>
	///   Gets element at the given index. If index is negative, the array is indexed from the last element.
	///   Using index -1 will get the last element, -2 the second last etc.
	///   Using index >= 0 will just use normal array indexing, so that index 0 is the first element, 1 the second element
	///   etc.
	/// </summary>
	public static T Get<T>(this T[] @this, int index) {
		if (index < 0) return @this[@this.Length + index];
		return @this[index];
	}

	public static T[] Copy<T>(this T[] @this) {
		var result = new T[@this.Length];
		Array.Copy(@this, result, result.Length);
		return result;
	}

	/// <summary>
	///   Merge 2 arrays sorted in ascending order.
	/// </summary>
	/// <returns>A new array with the contents of the sorted arrays merged.</returns>
	public static T[] MergeSorted<T>(this T[] x, T[] y, Comparison<T> comparison, bool skipDuplicates = false, bool strictOrdering = true) where T : IComparable<T> {
		if (x == null) throw new ArgumentNullException(nameof(x));
		if (y == null) throw new ArgumentNullException(nameof(y));
		if (comparison == null) throw new ArgumentNullException(nameof(comparison));
		if (x.Length == 0) return y.Copy();
		if (y.Length == 0) return x.Copy();
		var z = new T[x.Length + y.Length];
		var xi = 0;
		var yi = 0;
		var zi = 0;
		var strictOrderingValue = strictOrdering ? 1 : 0;
		while (xi < x.Length && yi < y.Length) {
			var compareResult = comparison(x[xi], y[yi]);
			switch (compareResult) {
				case -1:
					z[zi++] = x[xi++];
					if (xi < x.Length && x[xi].CompareTo(x[xi - 1]) < strictOrderingValue) throw new ArgumentException("Array x not sorted in ascending order.");
					break;
				case 0:
					z[zi++] = x[xi++];
					if (xi < x.Length && x[xi].CompareTo(x[xi - 1]) < strictOrderingValue) throw new ArgumentException("Array x not sorted in ascending order.");
					if (skipDuplicates) {
						do {
							yi++;
							if (yi < y.Length && y[yi].CompareTo(y[yi - 1]) < strictOrderingValue) throw new ArgumentException("Array y not sorted in ascending order.");
						} while (yi < y.Length && comparison(x[xi - 1], y[yi]) == 0);
					}
					break;
				case 1:
					z[zi++] = y[yi++];
					if (yi < y.Length && y[yi].CompareTo(y[yi - 1]) < strictOrderingValue) throw new ArgumentException("Array y not sorted in ascending order.");
					break;
				default: throw new Exception("compareResult = " + compareResult);
			}
		}
		while (xi < x.Length) {
			z[zi++] = x[xi++];
			if (xi >= x.Length) break;
			if (x[xi].CompareTo(x[xi - 1]) < strictOrderingValue) throw new ArgumentException("Array x not sorted in ascending order.");
		}
		while (yi < y.Length) {
			z[zi++] = y[yi++];
			if (yi >= y.Length) break;
			if (y[yi].CompareTo(y[yi - 1]) < strictOrderingValue) throw new ArgumentException("Array y not sorted in ascending order.");
		}
		if (zi < x.Length + y.Length) Array.Resize(ref z, zi);
		return z;
	}
}