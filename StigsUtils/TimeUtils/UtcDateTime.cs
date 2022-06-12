// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.
namespace StigsUtils.TimeUtils;

public struct UtcDateTime : IEquatable<UtcDateTime>, IComparable<UtcDateTime>, IComparable {
	public static UtcDateTime UtcNow => TimeProvider();
	internal static Func<UtcDateTime> TimeProvider { get; set; } = () => DateTime.UtcNow;
	private readonly DateTime _dateTime;
	public long Ticks => _dateTime.Ticks;
	public static UtcDateTime MaxValue { get; } = DateTime.MaxValue;
	public static UtcDateTime MinValue { get; } = DateTime.MinValue;

	public UtcDateTime(in long ticks) => _dateTime = new DateTime(ticks, DateTimeKind.Utc);

	public UtcDateTime(in DateTime dateTime) : this(dateTime.Ticks) { }

	public static implicit operator UtcDateTime(long ticks) => new UtcDateTime(ticks);

	public static implicit operator UtcDateTime(DateTime dateTime) => new UtcDateTime(dateTime);

	public static implicit operator DateTime(UtcDateTime utcDateTime) => utcDateTime._dateTime;

	public int CompareTo(UtcDateTime other) => _dateTime.CompareTo(other._dateTime);

	public int CompareTo(object? obj) {
		if (ReferenceEquals(null, obj)) return 1;
		return obj is UtcDateTime other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(UtcDateTime)}");
	}

	public static bool operator <(UtcDateTime left, UtcDateTime right) => left.CompareTo(right) < 0;

	public static bool operator >(UtcDateTime left, UtcDateTime right) => left.CompareTo(right) > 0;

	public static bool operator <=(UtcDateTime left, UtcDateTime right) => left.CompareTo(right) <= 0;

	public static bool operator >=(UtcDateTime left, UtcDateTime right) => left.CompareTo(right) >= 0;

	public bool Equals(UtcDateTime other) => _dateTime.Equals(other._dateTime);

	public override bool Equals(object? obj) => obj is UtcDateTime other && Equals(other);

	public override int GetHashCode() => _dateTime.GetHashCode();

	public static bool operator ==(UtcDateTime left, UtcDateTime right) => left.Equals(right);

	public static bool operator !=(UtcDateTime left, UtcDateTime right) => !left.Equals(right);

	public static TimeSpan operator -(UtcDateTime left, UtcDateTime right) => left._dateTime - right._dateTime;

	public static DateTime operator +(UtcDateTime left, TimeSpan right) => left._dateTime + right;

	private DateTime ToDateTime() => _dateTime;
}