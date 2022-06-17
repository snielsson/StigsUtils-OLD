// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System.Collections.Concurrent;
namespace StigsUtils.Extensions;

public static class EnumExtensions {
	private static readonly ConcurrentDictionary<Type, Dictionary<string, object>> EnumValues = new();
	private static readonly ConcurrentDictionary<Type, Dictionary<object, string>> EnumNames = new();

	public static TEnum Parse<TEnum>(this string value, bool ignoreCase = true) where TEnum : struct => (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);

	public static T GetEnumValue<T>(this Type @this, string name) {
		Dictionary<string, object> keyValueMap = EnumValues.GetOrAdd(@this, enumType => {
			var keys = Enum.GetNames(enumType);
			Array values = Enum.GetValues(enumType);
			var map = new Dictionary<string, object>();
			for (var i = 0; i < keys.Length; i++) map[keys[i]] = values.GetValue(i) ?? throw new InvalidOperationException();
			return map;
		});
		return (T)keyValueMap[name];
	}

	public static string GetEnumName(this Type @this, object val) {
		Dictionary<object, string> valueKeyMap = EnumNames.GetOrAdd(@this, enumType => {
			var keys = Enum.GetNames(enumType);
			Array values = Enum.GetValues(enumType);
			var map = new Dictionary<object, string>();
			for (var i = 0; i < keys.Length; i++) map[values.GetValue(i) ?? throw new InvalidOperationException()] = keys[i];
			return map;
		});
		return valueKeyMap[val];
	}
}