// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System.Collections.Concurrent;
using StigsUtils.DataTypes.Exceptions;
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
	
		public static bool IsEnumWithConsecutiveValues(this Type @this) {
			if (!@this.IsEnum) return false;
			var indexOfNonConsecutiveValue = GetIndexOfNonConsecutiveValue(@this);
			return indexOfNonConsecutiveValue == -1;
		}
		private static int GetIndexOfNonConsecutiveValue(Type type) {
			var values = Enum.GetValues(type).ToArray<long>();
			if (values.Length == 1) return -1;
			var prev = values[0];
			for (int i = 1; i < values.Length; i++) {
				var val = values[i];
				if (val != prev + 1) return i;
				prev = val;
			}
			return -1;
		}
		public static Type AssertIsEnumWithConsecutiveValues(this Type @this) {
			if(!@this.IsEnum) throw @this.Error($"The type {@this} is not an enumeration.");
			var indexOfNonConsecutiveValue = GetIndexOfNonConsecutiveValue(@this);
			if (indexOfNonConsecutiveValue == -1) return @this;
			Array values = Enum.GetValues(@this);
			string[] names = Enum.GetNames(@this);
			object prevValue = values.GetValue(indexOfNonConsecutiveValue - 1)!;
			var prevName = names[indexOfNonConsecutiveValue - 1];
			object value = values.GetValue(indexOfNonConsecutiveValue)!;
			var name = names[indexOfNonConsecutiveValue];
			if (!@this.IsEnumWithConsecutiveValues()) throw @this.Error($"The enum type {@this} does not have consecutive values from {prevName}({prevValue}) to {name}({value}).");
			return @this;
	}	
}