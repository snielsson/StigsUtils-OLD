// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
namespace StigsUtils.Extensions;

public static class TimeSpanExtensions {
	public static long AsMultiplesOf(this TimeSpan @this, TimeSpan timeSpan) => @this.Ticks.AsMultiplesOf(timeSpan.Ticks);

	public static bool IsMultipleOf(this TimeSpan @this, TimeSpan timeSpan) => @this.Ticks.IsMultipleOf(timeSpan.Ticks);

	public static TimeSpan ToTimeSpan(this string @this) => TimeSpan.Parse(@this);

	public static TimeSpan Ticks(this long @this) => TimeSpan.FromTicks(@this);

	public static TimeSpan Milliseconds(this double @this) => TimeSpan.FromMilliseconds(@this);

	public static TimeSpan Milliseconds(this int @this) => TimeSpan.FromMilliseconds(@this);

	public static TimeSpan Seconds(this double @this) => TimeSpan.FromSeconds(@this);

	public static TimeSpan Seconds(this int @this) => TimeSpan.FromSeconds(@this);

	public static TimeSpan Minutes(this double @this) => TimeSpan.FromMinutes(@this);

	public static TimeSpan Minutes(this int @this) => TimeSpan.FromMinutes(@this);

	public static TimeSpan Hours(this double @this) => TimeSpan.FromHours(@this);

	public static TimeSpan Hours(this int @this) => TimeSpan.FromHours(@this);

	public static TimeSpan Days(this double @this) => TimeSpan.FromDays(@this);

	public static TimeSpan Days(this int @this) => TimeSpan.FromDays(@this);

	public static string ToHumanString(this TimeSpan @this) {
		if (@this < TimeSpan.FromMilliseconds(1)) return $"{(int)@this.TotalMilliseconds * 1000} microseconds";
		if (@this < TimeSpan.FromSeconds(1)) return $"{(int)@this.TotalMilliseconds} milliseconds";
		if (@this < TimeSpan.FromMinutes(1)) return $"{(int)@this.TotalSeconds,2} seconds";
		if (@this < TimeSpan.FromHours(1)) return $"{(int)@this.TotalMinutes,2} minutes";
		if (@this < TimeSpan.FromDays(1)) return $"{(int)@this.TotalHours,2} hours";
		return $"{(int)@this.TotalDays,2} days";
	}
}