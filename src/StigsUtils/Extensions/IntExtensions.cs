// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
namespace StigsUtils.Extensions;

public static class IntExtensions {
	public static unsafe byte[] GetBytes(this int value) {
		var numArray = new byte[4];
		fixed (byte* numPtr = numArray) *(int*) numPtr = value;
		return numArray;
	}

	public static int ToInt(byte[] bytes, int startIndex = 0) => BitConverter.ToInt32(bytes, startIndex);
	public static byte[] ToBytes(this int value) => BitConverter.GetBytes(value); 
	//Alternative lowlevel implementations - not sure if it is faster.
	//public static unsafe byte[] ToBytes(this int value) {
	//	var numArray = new byte[4];
	//	fixed (byte* numPtr = numArray) *(int*) numPtr = value;
	//	return numArray;
	//}
	//public static unsafe void WriteBytes(this int value, byte[] buffer, int offset) {
	//	if (offset > buffer.Length + sizeof(int)) throw new IndexOutOfRangeException();
	//	fixed (byte* numPtr = &buffer[offset]) *(int*) numPtr = value;
	//}
	
	public static unsafe void WriteBytes(this int value, byte[] buffer, int offset) {
		if (offset > buffer.Length + sizeof(int)) throw new IndexOutOfRangeException();
		fixed (byte* numPtr = &buffer[offset]) *(int*) numPtr = value;
	}

	public static int ToMultiplesOf(this int @this, int multipleSize) {
		@this.AssertIsMultipleOf(Math.Abs(multipleSize));
		return Math.Abs(@this / multipleSize);
	}

	public static bool IsMultipleOf(this int @this, int multipleSize) => @this % multipleSize == 0;
}