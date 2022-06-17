// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
namespace StigsUtils.Extensions;

public static class LongExtensions {
	public static unsafe byte[] GetBytes(this long value) {
		var numArray = new byte[sizeof(long)];
		fixed (byte* numPtr = numArray) *(long*) numPtr = value;
		return numArray;
	}

	public static unsafe void WriteBytes(this long value, byte[] buffer, int offset) {
		if (offset > buffer.Length + sizeof(long)) throw new IndexOutOfRangeException();
		fixed (byte* numPtr = &buffer[offset]) *(long*) numPtr = value;
	}

	public static long AsMultiplesOf(this long @this, long multipleSize) {
		@this.DebugAssertIsMultipleOf(multipleSize);
		return @this / multipleSize;
	}

	public static bool IsMultipleOf(this long @this, long multipleSize) => @this % multipleSize == 0;

	public static DateTime AsDateTime(this int @this) => @this.FromUnixTime();
	public static DateTime AsDateTime(this long @this) => new DateTime(@this, DateTimeKind.Utc);
	public static DateTime AsDateTime(this ulong @this) => new DateTime((long) @this, DateTimeKind.Utc);
}