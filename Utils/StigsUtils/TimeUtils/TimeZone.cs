// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
namespace StigsUtils.TimeUtils;

public static class TimeZone {
	public static TimeZoneInfo? ByCity(string city) {
		switch (city.ToLower()) {
			case "cph":
			case "copenhagen":
			case "brussels":
			case "madrid":
			case "paris":
				return TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
			default:
				return null;
		}
	}

	public static TimeZoneInfo? ById(string id) {
		try {
			return TimeZoneInfo.FindSystemTimeZoneById(id);
		}
		catch (TimeZoneNotFoundException) {
			return null;
		}
	}

	public static TimeZoneInfo GetTimeZoneInfo(string s) => (ById(s) ?? ByCity(s)) ?? throw new InvalidOperationException();

	public static TimeZoneInfo ToTimeZoneInfo(this string @this) => (ById(@this) ?? ByCity(@this)) ?? throw new InvalidOperationException();
}