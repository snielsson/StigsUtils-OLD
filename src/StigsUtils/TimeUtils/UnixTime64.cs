// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using StigsUtils.Extensions;
namespace StigsUtils.TimeUtils;

public readonly struct UnixTime64 {
	private readonly DateTime _dateTime;

	public UnixTime64(DateTime dateTime) => _dateTime = dateTime;

	public UnixTime64(long l) => _dateTime = UnixTime32.UnixEpoch.AddMilliseconds(l);

	public int Value => _dateTime.ToUnixTime();

	public UnixTime64 AddDays(double x) => new UnixTime64(_dateTime.AddDays(x));

	public static implicit operator int(UnixTime64 x) => x.Value;

	public static implicit operator DateTime(UnixTime64 x) => x._dateTime;

	public static explicit operator UnixTime32(UnixTime64 x) => new UnixTime32(x._dateTime);
}