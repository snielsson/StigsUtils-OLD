// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using StigsUtils.Extensions;
namespace StigsUtils.TimeUtils;

public readonly struct UnixTime32 {
	private readonly DateTime _dateTime;
	public const int UnixEpochYear = 1970;
	public const int UnixEpochMonth = 1;
	public const int UnixEpochDay = 1;
	public static readonly DateTime UnixEpoch = new DateTime(UnixEpochYear, UnixEpochMonth, UnixEpochDay, 0, 0, 0, DateTimeKind.Utc);

	public UnixTime32(DateTime dateTime) => _dateTime = dateTime;

	public UnixTime32(int secondsSinceEpoch) => _dateTime = secondsSinceEpoch.FromUnixTime();

	public UnixTime32(int y, int m, int d, int h = 0, int min = 0, int s = 0) => _dateTime = new DateTime(y, m, d, h, min, s, DateTimeKind.Utc);

	public int Value => _dateTime.ToUnixTime();
	public UnixTime32 Date => new UnixTime32(_dateTime.Date);
	public int Year => _dateTime.Year;
	public int Month => _dateTime.Month;
	public int Day => _dateTime.Day;
	public static readonly UnixTime32 MaxValue = new UnixTime32(int.MaxValue);
	public static readonly UnixTime32 MinValue = new UnixTime32(0);

	public UnixTime32 AddDays(double x) => new UnixTime32(_dateTime.AddDays(x));

	public static TimeSpan operator -(UnixTime32 lhs, UnixTime32 rhs) => lhs._dateTime - rhs._dateTime;

	public static explicit operator int(UnixTime32 x) => x.Value;

	public static explicit operator DateTime(UnixTime32 x) => x._dateTime;

	public static explicit operator UnixTime64(UnixTime32 x) => new UnixTime64(x._dateTime);
}