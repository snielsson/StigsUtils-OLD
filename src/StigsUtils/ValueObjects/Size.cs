// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
namespace StigsUtils.ValueObjects;

/// <summary>
/// Size is an Int32 value that is not negative and which supports:
///		- arithmetic operators on Int32 and Int64 
///		- comparison operators on Int32 and Int64
///		- implicit casting from Int32
///		- explicit casting to Int32 and Int64
/// </summary>
public readonly record struct Size {
	public Size(int value) {
		if (value < 0) throw new ArgumentException($"Value {value} is negative.",nameof(value));
		Value = value;
	}
	public static implicit operator Size(int x) => new Size(x);
	public static explicit operator int(Size x) => x.Value;
	public static explicit operator long(Size x) => x.Value;
	public static int operator -(Size x, int y) => x.Value - y;
	public static int operator -(int x, Size y) => x - y.Value;
	public static int operator +(Size x, int y) => x.Value + y;
	public static int operator +(int x, Size y) => x + y.Value;
	public static int operator *(Size x, int y) => x.Value * y;
	public static int operator *(int x, Size y) => x * y.Value;
	public static int operator /(Size x, int y) => x.Value / y;
	public static int operator /(int x, Size y) => x / y.Value;
	public static int operator %(Size x, int y) => x.Value % y;
	public static int operator %(int x, Size y) => x % y.Value;
	
	public static bool operator ==(Size x, int y) => x.Value == y;
	public static bool operator !=(Size x, int y) => x.Value != y;
	public static bool operator >(Size x, int y) => x.Value > y;
	public static bool operator <(Size x, int y) => x.Value < y;
	public static bool operator >(int x, Size y) => x > y.Value;
	public static bool operator <(int x, Size y) => x < y.Value;
	public static bool operator >=(int x, Size y) => x >= y.Value;
	public static bool operator <=(int x, Size y) => x <= y.Value;
	public static bool operator >=(Size x, int y) => x.Value >= y;
	public static bool operator <=(Size x, int y) => x.Value <= y;
	
	public static long operator -(Size x, long y) => x.Value - y;
	public static long operator -(long x, Size y) => x - y.Value;
	public static long operator +(Size x, long y) => x.Value + y;
	public static long operator +(long x, Size y) => x + y.Value;
	public static long operator *(Size x, long y) => x.Value * y;
	public static long operator *(long x, Size y) => x * y.Value;
	public static long operator /(Size x, long y) => x.Value / y;
	public static long operator /(long x, Size y) => x / y.Value;

	public static long operator %(Size x, long y) => x.Value % y;
	public static long operator %(long x, Size y) => x % y.Value;
	public static bool operator ==(Size x, long y) => x.Value == y;
	public static bool operator !=(Size x, long y) => x.Value != y;
	public static bool operator >(Size x, long y) => x.Value > y;
	public static bool operator <(Size x, long y) => x.Value < y;
	public static bool operator >(long x, Size y) => x > y.Value;
	public static bool operator <(long x, Size y) => x < y.Value;
	public static bool operator >=(long x, Size y) => x >= y.Value;
	public static bool operator <=(long x, Size y) => x <= y.Value;
	public static bool operator >=(Size x, long y) => x.Value >= y;
	public static bool operator <=(Size x, long y) => x.Value <= y;
	public int Value { get; init; }
	public static Size MaxValue { get; } = new (int.MaxValue);
	public static Size MinValue { get; } = new (0);
	public static Size Default { get; } = MinValue;
};