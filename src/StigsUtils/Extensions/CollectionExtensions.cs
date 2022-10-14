// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.
using System.Collections;
namespace StigsUtils.Extensions;

public static class CollectionExtensions {
	
	public static ICollection<T> Add<T>(this ICollection<T> @this, T item) {
		@this.Add(item);
		return @this;
	}
	
	public static ICollection<T> AddTo<T>(this T @this, ICollection<T> collection) { 
		collection.Add(@this);
		return collection;
	}
		
	public static bool IsNullOrEmpty<T>(this ICollection<T>? @this) {
		if (@this == null) return true;
		return @this.Count == 0;
	}

	public static bool IsEmpty<T>(this ICollection<T> @this) {
		if (@this == null) throw new ArgumentNullException(nameof(@this));
		return @this.Count == 0;
	}

	public static bool IsNotEmpty<T>(this ICollection<T> @this) => !@this.IsEmpty();

	//https://www.jacksondunstan.com/articles/3189

	/// <summary>
	///   Insert a value into an IList{T} that is presumed to be already sorted such that sort
	///   ordering is preserved
	/// </summary>
	/// <param name="list">List to insert into</param>
	/// <param name="value">Value to insert</param>
	/// <returns>Index where value was inserted.</returns>
	/// ///
	/// <typeparam name="T">Type of element to insert and type of elements in the list</typeparam>
	public static int InsertSorted<T>(this IList<T> list, T value) where T : IComparable<T> => InsertSorted(list, value, (a, b) => a.CompareTo(b));

	/// <summary>
	///   Insert a value into an IList{T} that is presumed to be already sorted such that sort
	///   ordering is preserved
	/// </summary>
	/// <param name="list">List to insert into</param>
	/// <param name="value">Value to insert</param>
	/// <param name="comparison">Comparison to determine sort order with</param>
	/// <returns>Index where value was inserted.</returns>
	/// <typeparam name="T">Type of element to insert and type of elements in the list</typeparam>
	public static int InsertSorted<T>(this IList<T> list, T value, Comparison<T> comparison) {
		var startIndex = 0;
		var endIndex = list.Count;
		while (endIndex > startIndex) {
			var windowSize = endIndex - startIndex;
			var middleIndex = startIndex + windowSize / 2;
			T middleValue = list[middleIndex];
			var compareToResult = comparison(middleValue, value);
			if (compareToResult == 0) {
				list.Insert(middleIndex, value);
				return middleIndex;
			}
			if (compareToResult < 0) startIndex = middleIndex + 1;
			else endIndex = middleIndex;
		}
		list.Insert(startIndex, value);
		return startIndex;
	}

	/// <summary>
	///   Insert a value into an IList that is presumed to be already sorted such that sort ordering is preserved
	/// </summary>
	/// <param name="list">List to insert into</param>
	/// <param name="value">Value to insert</param>
	/// <returns>Index where value was inserted.</returns>
	public static int InsertSorted(this IList list, IComparable value) => InsertSorted(list, value, (a, b) => a.CompareTo(b));

	/// <summary>
	///   Insert a value into an IList that is presumed to be already sorted such that sort ordering is preserved
	/// </summary>
	/// <param name="list">List to insert into</param>
	/// <param name="value">Value to insert</param>
	/// <param name="comparison">Comparison to determine sort order with</param>
	/// <returns>Index where value was inserted.</returns>
	public static int InsertSorted(this IList list, IComparable value, Comparison<IComparable> comparison) {
		var startIndex = 0;
		var endIndex = list.Count;
		while (endIndex > startIndex) {
			var windowSize = endIndex - startIndex;
			var middleIndex = startIndex + windowSize / 2;
			var middleValue = (IComparable)list[middleIndex]!;
			var compareToResult = comparison(middleValue, value);
			if (compareToResult == 0) {
				list.Insert(middleIndex, value);
				return middleIndex;
			}
			if (compareToResult < 0) startIndex = middleIndex + 1;
			else endIndex = middleIndex;
		}
		list.Insert(startIndex, value);
		return startIndex;
	}
	
	
}