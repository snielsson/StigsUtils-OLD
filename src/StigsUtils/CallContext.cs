// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System.Collections.Concurrent;
namespace StigsUtils;

public class CallContext {

	private static readonly AsyncLocal<ConcurrentDictionary<string, object>> Properties = new();

	public static T? Get<T>(string key) {
		if (Properties.Value!.TryGetValue(key, out var val)) {
			return (T)val;
		}
		return default;
	}

	public static void Set(string key, object val) {
		Properties.Value![key] = val;
	}

	public static bool TryGet<T>(string key, out T? result) {
		if (Properties.Value!.TryGetValue(key, out var val)) {
			result = (T)val;
			return true;
		}
		result = default;
		return false;
	}
}