// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.
using System.Collections;
namespace StigsUtils.Extensions;

public static class LinqExtensions {
	public static bool In<T>(this T @this, params T[]? arg) {
		return arg?.Any(x => x?.Equals(@this) ?? false) ?? true;
		// Null arg is ignore, ie. does no filtering.
	}

	public static bool In<T>(this T @this, IEnumerable<T>? arg) {
		return arg?.Any(x => x?.Equals(@this) ?? false) ?? true;
	}

	public static IEnumerable<T> NotNull<T>(this IEnumerable<T> @this) {
		return @this.Where(x => x != null);
	}

	public static bool IsNullOrEmpty<T>(this IEnumerable<T>? @this) => !@this?.Any() ?? true;

	public static bool IsEmpty<T>(this IEnumerable<T> @this) => !@this.Any();

	public static IEnumerable<T> Concatenate<T>(this IEnumerable<T> @this, params IEnumerable<T>[] args) {
		return @this.Concat(args.SelectMany(x => x));
	}

	public static IEnumerable<T> Concatenate<T>(params IEnumerable<T>[] args) {
		return args.SelectMany(x => x);
	}

	public static IEnumerable<T> Map<T>(this IEnumerable<T> @this, Func<T, T> func) => @this.Select(func);

	public static T ForEach<T>(this IEnumerable<T> @this, Func<T, bool> predicate, Action<T> action) => @this.Where(predicate).ForEach(action);

	public static T ForEach<T>(this IEnumerable<T> @this, Action<T> action) {
		T last = default!;
		foreach (T item in @this) action(last = item);
		return last;
	}

	public static TOutput ForEach<TInput, TOutput>(this IEnumerable<TInput> @this, Func<TInput, TOutput> func) {
		TOutput last = default!;
		foreach (TInput item in @this) last = func(item);
		return last;
	}

	public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T>? @this) => @this ?? Array.Empty<T>();

	public static IEnumerable<T> SkipNulls<T>(this IEnumerable<T> @this) {
		return @this.Where(x => x != null);
	}

	public static IReadOnlyList<T> ToReadOnlyList<T>(this IEnumerable @this, Func<object?, T> convert) => @this.Cast<object?>().Select(convert).ToList();

	public static IReadOnlyDictionary<TKey, IEnumerable<TValue>> ToReadOnlyDictionary<TKey, TValue>(this IEnumerable<IGrouping<TKey, TValue>>? @this) where TKey : notnull {
		var result = new Dictionary<TKey, IEnumerable<TValue>>();
		if (@this == null) return result;
		foreach (IGrouping<TKey, TValue>? grouping in @this) {
			if (grouping == null) continue;
			result.Add(grouping.Key, grouping);
		}
		return result;
	}

	public static IReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>(this IEnumerable? @this, Func<TValue, TKey> keyGetter) where TKey : notnull {
		var result = new Dictionary<TKey, TValue>();
		if (@this == null) return result;
		foreach (var item in @this) {
			if (item == null) continue;
			var value = (TValue)item;
			result.Add(keyGetter(value), value);
		}
		return result;

	}

	public static IReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TData, TKey, TValue>(this IEnumerable? @this, Func<TData, TKey> keyGetter, Func<TData, TValue> valueGetter) where TKey : notnull {
		var result = new Dictionary<TKey, TValue>();
		if (@this == null) return result;
		foreach (var item in @this) {
			if (item == null) continue;
			var data = (TData)item;
			result.Add(keyGetter(data), valueGetter(data));
		}
		return result;
	}

	public static IReadOnlyDictionary<TKey, TValue>? ToReadOnlyDictionary<TKey, TValue>(this IEnumerable<TValue>? @this, Func<TValue, TKey> keyGetter) where TKey : notnull {
		if (@this == null) return null;
		var result = new Dictionary<TKey, TValue>();
		foreach (TValue value in @this) {
			result.Add(keyGetter(value), value);
		}
		return result;
	}

	public static IEnumerable OrEmpty(this IEnumerable? @this) => @this ?? Array.Empty<object>();
}