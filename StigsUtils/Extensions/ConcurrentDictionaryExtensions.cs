// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.
using System.Collections.Concurrent;
namespace StigsUtils.Extensions;

public static class ConcurrentDictionaryExtensions {
	public static ConcurrentDictionary<TKey, TValue> Add<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> @this, TKey key, TValue val) where TKey : notnull {
		if (!@this.TryAdd(key, val)) throw new Exception($"Failed to add value '{val}' on key '{key}'.");
		return @this;
	}
}