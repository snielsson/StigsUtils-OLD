// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.
namespace StigsUtils.Extensions;

public static class EnumExtensions {

	public static TEnum Parse<TEnum>(this string value, bool ignoreCase = true) where TEnum : struct => (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);
}