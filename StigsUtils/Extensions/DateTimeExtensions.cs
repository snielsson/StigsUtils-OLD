// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.
namespace StigsUtils.Extensions;

public static class DateTimeExtensions {
	public static DateTime NextIntervalStart(this DateTime @this, TimeSpan interval)
		=> @this.IntervalStart(interval) + interval;

	public static DateTime IntervalStart(this DateTime @this, TimeSpan interval) {
		var ticks = @this.Ticks / interval.Ticks * interval.Ticks;
		var result = new DateTime(ticks, @this.Kind);
		return result;
	}

	public static DateTime ToDate(this int @this) {
		var year = @this / 10000;
		var month = @this % 10000 / 100;
		var day = @this % 100;
		return new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);
	}

	public static DateTime? ToDate(this int? @this) {
		if (@this == null) return null;
		var year = @this.Value / 10000;
		var month = @this.Value % 10000 / 100;
		var day = @this.Value % 100;
		return new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);
	}

	public static int ToIntDate(this DateTime @this) => @this.Year * 10000 + @this.Month * 100 + @this.Day;
}