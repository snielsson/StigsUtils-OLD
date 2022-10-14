// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
namespace StigsUtils.Extensions;

public static class ListExtensions {

	/// <summary>
	/// Performs a binary search on the specified ordered collection.
	/// </summary>
	/// <typeparam name="TItem">The type of the item.</typeparam>
	/// <param name="this">The list to be searched.</param>
	/// <param name="value">The value to search for.</param>
	/// <returns></returns>
	public static int BinarySearch<TItem>(this IList<TItem> @this, TItem value) => BinarySearch(@this, value, Comparer<TItem>.Default);

	/// <summary>
	///     Performs a binary search on the specified ordered collection.
	/// </summary>
	/// <typeparam name="TItem">The type of the item.</typeparam>
	/// <param name="this">The list to be searched.</param>
	/// <param name="value">The value to search for.</param>
	/// <param name="comparer">The comparer that is used to compare the value with the list items.</param>
	/// <returns></returns>
	public static int BinarySearch<TItem>(this IList<TItem> @this, TItem value, IComparer<TItem> comparer) => @this.BinarySearch(value, comparer.Compare);

	public static IList<T> InsertSorted<T>(this IList<T> @this, T value, IComparer<T>? comparer = null, bool strictSorting = false) {
		comparer ??= Comparer<T>.Default;
		var index = @this.BinarySearch(value,comparer);
		if (strictSorting && index >= 0) throw new ArgumentException($"Value is already at index {index}.");
		if (index < 0) index = ~index;
		@this.Insert(index < 0 ? ~index : index, value);
		return @this;
	}

	/// <summary>
	/// Performs a binary search on the specified ordered collection.
	/// </summary>
	/// <typeparam name="TItem">The type of the item.</typeparam>
	/// <typeparam name="TSearch">The type of the searched item.</typeparam>
	/// <param name="this">The list to be searched.</param>
	/// <param name="value">The value to search for.</param>
	/// <param name="comparer">The comparer that is used to compare the value with the list items.</param>
	/// <returns>Index of item.</returns>
	public static int BinarySearch<TItem, TSearch>(this IList<TItem> @this, TSearch value, Func<TSearch, TItem, int> comparer) {
		if (@this == null) throw new ArgumentNullException(nameof(@this));
		if (comparer == null) throw new ArgumentNullException(nameof(comparer));
		var lower = 0;
		var upper = @this.Count - 1;
		while (lower <= upper) {
			var middle = lower + (upper - lower) / 2;
			var comparisonResult = comparer(value, @this[middle]);
			if (comparisonResult < 0) upper = middle - 1;
			else if (comparisonResult > 0) lower = middle + 1;
			else return middle;
		}
		return ~lower;
	}

	/*
		/// <summary>
		///     Performs a binary search on the specified collection.
		/// </summary>
		/// <typeparam name="TItem">The type of the item.</typeparam>
		/// <typeparam name="TSearch">The type of the searched item.</typeparam>
		/// <param name="list">The list to be searched.</param>
		/// <param name="value">The value to search for.</param>
		/// <param name="comparer">
		///     The comparer that is used to compare the value
		///     with the list items.
		/// </param>
		/// <returns></returns>
		public static int BinarySearch<TItem, TSearch>(this IList<TItem> list, TSearch value, Func<TSearch, TItem, int> comparer) {
			if (list == null) throw new ArgumentNullException("list");
			if (comparer == null) throw new ArgumentNullException("comparer");
			var lower = 0;
			var upper = list.Count - 1;
			while (lower <= upper) {
				var middle = lower + (upper - lower) / 2;
				var comparisonResult = comparer(value, list[middle]);
				if (comparisonResult < 0) {
					upper = middle - 1;
				}
				else if (comparisonResult > 0) {
					lower = middle + 1;
				}
				else {
					return middle;
				}
			}
			return ~lower;
		}

		/// <summary>
		///     Performs a binary search on the specified collection.
		/// </summary>
		/// <typeparam name="TItem">The type of the item.</typeparam>
		/// <param name="list">The list to be searched.</param>
		/// <param name="value">The value to search for.</param>
		/// <returns></returns>
		public static int BinarySearch<TItem>(this IList<TItem> list, TItem value) => BinarySearch(list, value, Comparer<TItem>.Default);

		/// <summary>
		///     Performs a binary search on the specified collection.
		/// </summary>
		/// <typeparam name="TItem">The type of the item.</typeparam>
		/// <param name="list">The list to be searched.</param>
		/// <param name="value">The value to search for.</param>
		/// <param name="comparer">
		///     The comparer that is used to compare the value
		///     with the list items.
		/// </param>
		/// <returns></returns>
		public static int BinarySearch<TItem>(this IList<TItem> list, TItem value, IComparer<TItem> comparer) => list.BinarySearch(value, comparer.Compare);
	
	*/
	
}