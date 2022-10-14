// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System.Diagnostics;
using System.Runtime.InteropServices;
namespace StigsUtils.Extensions;

public static class ByteArrayExtensions {
	public const int DefaultByteChunkSize = 4096;

	public static byte[] BlitToByteArray<T>(this T[] @this, int offset = 0, int length = 0) {
		var marshalledSize = Marshal.SizeOf<T>();
		var bytes = new byte[(length == 0 ? @this.Length : length) * marshalledSize];
		Debug.Assert(bytes.Length.IsMultipleOf(marshalledSize), $"Expected number of bytes ({bytes.Length}) to be a multiple of the marshalled size ({marshalledSize}) of {typeof(T)}.");
		GCHandle pinnedHandle = default(GCHandle);
		try {
			pinnedHandle = GCHandle.Alloc(@this, GCHandleType.Pinned);
			Marshal.Copy(pinnedHandle.AddrOfPinnedObject() + offset * marshalledSize, bytes, 0, bytes.Length);
			return bytes;
		}
		finally {
			if (pinnedHandle.IsAllocated) pinnedHandle.Free();
		}
	}

	// public static T[] BlitFromByteArray<T>(this byte[] @this, int byteSizeOfElements) {
	// 	GCHandle pinnedHandle = default(GCHandle);
	// 	try {
	// 		var count = @this.Length.AsMultiplesOf(byteSizeOfElements);
	// 		var result = new T[count];
	// 		pinnedHandle = GCHandle.Alloc(result, GCHandleType.Pinned);
	// 		Marshal.Copy(@this, 0, pinnedHandle.AddrOfPinnedObject(), @this.Length);
	// 		return result;
	// 	}
	// 	finally {
	// 		if (pinnedHandle.IsAllocated) pinnedHandle.Free();
	// 	}
	// }	
	
	public static int ReadBigEndianInt32(this byte[] @this, int offset = 0) {
		return (@this[offset+0] << 24) | (@this[offset+1] << 16) | (@this[offset+2] << 8) | @this[offset+3];
	}	
	
	public static T[] BlitFromBytes<T>(this byte[] @this, int size, T[]? destination = null) {
		var count = @this.Length / size;
		var byteCount = count * size;
		if (@this.Length != byteCount) throw new InvalidOperationException($"Source array does not contain a multiple of {size} bytes.");
		if (destination == null) destination = new T[count];
		else if (destination.Length * size < @this.Length) throw new InvalidOperationException($"Destination array to small to hold {count} elements.");
		GCHandle pinnedHandle = GCHandle.Alloc(destination, GCHandleType.Pinned);
		try {
			Marshal.Copy(@this, 0, pinnedHandle.AddrOfPinnedObject(), byteCount);
		}
		finally {
			pinnedHandle.Free();
		}
		return destination;
	}

	public static byte[] BlitToBytes<T>(this T[] @this, int size, byte[]? destination = null) {
		var byteCount = @this.Length * size;
		if (destination == null) destination = new byte[byteCount];
		else if (destination.Length < byteCount) throw new InvalidOperationException($"Destination array to small to hold {byteCount} bytes.");
		GCHandle pinnedHandle = GCHandle.Alloc(@this, GCHandleType.Pinned);
		try {
			Marshal.Copy(pinnedHandle.AddrOfPinnedObject(), destination, 0, byteCount);
		}
		finally {
			pinnedHandle.Free();
		}
		return destination;
	}

	public static byte[] InsertBytes(this byte[] @this, long val, int startIndex = 0, bool? isLittleEndian = null) {
		isLittleEndian ??= BitConverter.IsLittleEndian;
		if (isLittleEndian.Value) {
			var limit = startIndex + sizeof(long);
			for (var i = startIndex; i < limit; i++) {
				@this[i] = unchecked((byte)(val & 0xff));
				val = val >> 8;
			}
		}
		else {
			for (var i = startIndex + sizeof(long) - 1; i >= startIndex; i--) {
				@this[i] = unchecked((byte)(val & 0xff));
				val = val >> 8;
			}
		}
		return @this;
	}

	public static byte[] InsertBytes(this byte[] @this, int val, int startIndex = 0, bool? isLittleEndian = null) {
		isLittleEndian ??= BitConverter.IsLittleEndian;
		if (isLittleEndian.Value) {
			var limit = startIndex + sizeof(int);
			for (var i = startIndex; i < limit; i++) {
				@this[i] = unchecked((byte)(val & 0xff));
				val = val >> 8;
			}
		}
		else {
			for (var i = startIndex + sizeof(int) - 1; i >= startIndex; i--) {
				@this[i] = unchecked((byte)(val & 0xff));
				val = val >> 8;
			}
		}
		return @this;
	}

	public static long GetLong(this byte[] @this, int startIndex = 0, bool? isLittleEndian = null) {
		isLittleEndian ??= BitConverter.IsLittleEndian;
		long result = 0;
		if (isLittleEndian.Value) {
			for (var i = startIndex + sizeof(long) - 1; i >= startIndex; i--) {
				result = (result << 8) | @this[i];
			}
		}
		else {
			var limit = startIndex + sizeof(long);
			for (var i = startIndex; i < limit; i++) {
				result = (result << 8) | @this[i];
			}
		}
		return result;
	}

	public static int GetInt(this byte[] @this, int startIndex = 0, bool? isLittleEndian = null) {
		isLittleEndian ??= BitConverter.IsLittleEndian;
		int result = 0;
		if (isLittleEndian.Value) {
			for (var i = startIndex + sizeof(int) - 1; i >= startIndex; i--) {
				result = (result << 8) | @this[i];
			}
		}
		else {
			var limit = startIndex + sizeof(long);
			for (var i = startIndex; i < limit; i++) {
				result = (result << 8) | @this[i];
			}
		}
		return result;
	}
	
	public static unsafe byte[] GetBytes(this ushort value) {
		var numArray = new byte[sizeof(ushort)];
		fixed (byte* numPtr = numArray) *(ushort*) numPtr = value;
		return numArray;
	}

	public static unsafe void WriteBytes(this ushort value, byte[] buffer, int offset) {
		if (offset > buffer.Length + sizeof(ushort)) throw new IndexOutOfRangeException();
		fixed (byte* numPtr = &buffer[offset]) *(ushort*) numPtr = value;
	}

	public static unsafe byte[] GetBytes(this short value) {
		var numArray = new byte[sizeof(short)];
		fixed (byte* numPtr = numArray) *(short*) numPtr = value;
		return numArray;
	}

	public static unsafe void WriteBytes(this short value, byte[] buffer, int offset) {
		if (offset > buffer.Length + sizeof(short)) throw new IndexOutOfRangeException();
		fixed (byte* numPtr = &buffer[offset]) *(short*) numPtr = value;
	}
}	
