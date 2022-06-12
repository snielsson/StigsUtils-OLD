// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.
using System.Collections.Concurrent;
namespace StigsUtils.Extensions;

public static class DictionaryExtensions {
	public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key, TValue value) where TKey : notnull {
		if (!@this.ContainsKey(key)) @this.Add(key, value);
		return value;
	}

	public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key, Func<TValue> factory) where TKey : notnull {
		TValue val;
		if (!@this.TryGetValue(key, out val!)) @this.Add(key, val = factory());
		return val;
	}

	public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key, Func<TKey, TValue> factory) where TKey : notnull {
		TValue val;
		if (!@this.TryGetValue(key, out val!)) @this.Add(key, val = factory(key));
		return val;
	}

	public static List<TValue> AddToList<TKey, TValue>(this IDictionary<TKey, List<TValue>> @this, TKey key, params TValue[] values) where TKey : notnull {
		if (!@this.TryGetValue(key, out List<TValue>? list)) list = @this[key] = new List<TValue>();
		list.AddRange(values);
		return list;
	}

	public static Dictionary<TKey2, TValue> AddToDictionary<TKey, TKey2, TValue>(this IDictionary<TKey, Dictionary<TKey2, TValue>> @this, TKey key, TKey2 key2, TValue value) where TKey : notnull where TKey2 : notnull {
		if (!@this.TryGetValue(key, out Dictionary<TKey2, TValue>? dict)) dict = @this[key] = new Dictionary<TKey2, TValue>();
		dict.Add(key2, value);
		return dict;
	}

	public static IDictionary<TKey, ICollection<TValue>> AddToCollection<TKey, TValue>(this IDictionary<TKey, ICollection<TValue>> @this, TKey key, params TValue[] values) where TKey : notnull {
		if (!@this.TryGetValue(key, out ICollection<TValue>? collection)) collection = @this[key] = new List<TValue>();
		foreach (TValue v in values) collection.Add(v);
		return @this;
	}

	public static ConcurrentDictionary<TKey, TValue> ToConcurrentDictionary<TKey, TValue>(this IEnumerable<TValue> values, Func<TValue, TKey> keySelector) where TKey : notnull {
		var result = new ConcurrentDictionary<TKey, TValue>();
		foreach (TValue value in values) {
			result[keySelector(value)] = value;
		}
		return result;
	}

	public static IDictionary<TKey, TValue> Insert<TKey, TValue>(this IDictionary<TKey, TValue> @this, IEnumerable<(TKey Key, TValue Value)>? items) where TKey : notnull {
		if (items == null) return @this;
		foreach ((TKey key, TValue value) in items) {
			@this[key] = value;
		}
		return @this;
	}

	public static IDictionary<TKey, TValue> Insert<TKey, TValue>(this IDictionary<TKey, TValue> @this, IEnumerable<TValue>? values, Func<TValue, TKey> keyGetter) where TKey : notnull {
		if (values == null) return @this;
		foreach (TValue value in values) {
			TKey key = keyGetter(value);
			@this[key] = value;
		}
		return @this;
	}

	public static IDictionary<TKey, TValue> Insert<TKey, TValue, TItem>(this IDictionary<TKey, TValue> @this, IEnumerable<TItem>? items, Func<TItem, TKey> keyGetter, Func<TItem, TValue> valueGetter) where TKey : notnull {
		if (items == null) return @this;
		foreach (TItem item in items) {
			@this[keyGetter(item)] = valueGetter(item);
		}
		return @this;
	}
}