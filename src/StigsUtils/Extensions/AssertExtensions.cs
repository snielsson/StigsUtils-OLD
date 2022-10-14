// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System.Diagnostics;
namespace StigsUtils.Extensions;

public static class AssertExtensions {
	#region DateTime assertion extensions

	public static void MustBeUtc(this DateTime @this) {
		if (@this.Kind != DateTimeKind.Utc) throw new Exception($"{@this} is not UTC but {@this.Kind}.");
	}

	#endregion

	#region DirectoryInfo assertion extensions

	public static void AssertIsNotReadOnly(this DirectoryInfo @this) {
		if (!@this.Exists) {
			if (@this.Parent == null) throw new Exception($"{@this} is does not exist and has no parent directory.");
			@this.Parent.AssertIsNotReadOnly();
		}
		else if (@this.Attributes.HasFlag(FileAttributes.ReadOnly)) throw new Exception($"{@this} is readonly.");
	}

	#endregion

	#region IComparable<T> assertion extensions

	[Conditional("DEBUG")]
	public static void DebugAssertEqualTo<T>(this IComparable<T> @this, T other, Func<string>? msg = null) {
		if (@this.CompareTo(other) != 0) throw new Exception(msg?.Invoke() ?? $"{@this} is not equal to {other}.");
	}

	public static void AssertEqualTo<T>(this IComparable<T> @this, T other, Func<string>? msg = null) {
		if (@this.CompareTo(other) != 0) throw new Exception(msg?.Invoke() ?? $"{@this} is not equal to {other}.");
	}

	public static void AssertBelongsTo<T>(this IComparable<T> @this, T start, T end, bool includeStart = true, bool includeEnd = true) {
		if (includeStart) {
			if (@this.CompareTo(start) < 0) throw new Exception($"{@this} does not belong to [ {start} ; {end} {(includeEnd ? "]" : "[")}.");
		}
		else {
			if (@this.CompareTo(start) <= 0) throw new Exception($"{@this} does not belong to ] {start} ; {end} {(includeEnd ? "]" : "[")}.");
		}
		if (includeEnd) {
			if (@this.CompareTo(end) > 0) throw new Exception($"{@this} does not belong to {(includeStart ? "[" : "]")} ; {end} ].");
		}
		else {
			if (@this.CompareTo(end) >= 0) throw new Exception($"{@this} does not belong to {(includeStart ? "[" : "]")} ; {end} [.");
		}
	}

	[Conditional("DEBUG")]
	public static void DebugAssertIsLessThan<T>(this IComparable<T> @this, T other) {
		if (@this.CompareTo(other) >= 0) throw new Exception($"{@this} is not less than {other}.");
	}

	public static void AssertIsLessThan<T>(this IComparable<T> @this, T other) {
		if (@this.CompareTo(other) >= 0) throw new Exception($"{@this} is not less than {other}.");
	}

	public static T WhichIsLessThan<T>(this T @this, T other) where T : IComparable<T> {
		AssertIsLessThan(@this, other);
		return @this;
	}

	[Conditional("DEBUG")]
	public static void DebugAssertIsLessThanOrEqualTo<T>(this IComparable<T> @this, T other) {
		if (@this.CompareTo(other) > 0) throw new Exception($"{@this} is not less than or equal to {other}.");
	}

	public static void AssertIsLessThanOrEqualTo<T>(this IComparable<T> @this, T other) {
		if (@this.CompareTo(other) > 0) throw new Exception($"{@this} is not less than or equal to {other}.");
	}

	public static T WhichIsLessThanOrEqualTo<T>(this T @this, T other) where T : IComparable<T> {
		AssertIsLessThanOrEqualTo(@this, other);
		return @this;
	}

	[Conditional("DEBUG")]
	public static void DebugAssertIsGreaterThan<T>(this IComparable<T> @this, T other) {
		if (@this.CompareTo(other) <= 0) throw new Exception($"{@this} is not greater than {other}.");
	}

	public static void AssertIsGreaterThan<T>(this IComparable<T> @this, T other) {
		if (@this.CompareTo(other) <= 0) throw new Exception($"{@this} is not greater than {other}.");
	}

	public static T WhichIsGreaterThan<T>(this T @this, T other) where T : IComparable<T> {
		AssertIsGreaterThan(@this, other);
		return @this;
	}

	[Conditional("DEBUG")]
	public static void DebugAssertIsGreaterThanOrEqualTo<T>(this IComparable<T> @this, T other) {
		if (@this.CompareTo(other) < 0) throw new Exception($"{@this} is not greater than or equal to {other}.");
	}

	public static void AssertIsGreaterThanOrEqualTo<T>(this IComparable<T> @this, T other) {
		if (@this.CompareTo(other) < 0) throw new Exception($"{@this} is not greater than or equal to {other}.");
	}

	public static T WhichIsGreaterThanOrEqualTo<T>(this T @this, T other) where T : IComparable<T> {
		AssertIsGreaterThanOrEqualTo(@this, other);
		return @this;
	}

	#endregion

	#region FileInfo assertion extensions

	[Conditional("DEBUG")]
	public static void DebugAssertIsNotReadOnly(this FileInfo @this) {
		if (@this.IsReadOnly) throw new Exception($"{@this} is readonly.");
	}

	public static void AssertIsNotReadOnly(this FileInfo @this) {
		if (!@this.Exists) throw new Exception($"{@this} does not exist.");
		else if (@this.IsReadOnly) throw new Exception($"{@this} is readonly.");
	}

	public static FileInfo WhichIsNotReadOnly(this FileInfo @this) {
		if (@this.IsReadOnly) throw new Exception($"{@this} is readonly.");
		return @this;
	}

	[Conditional("DEBUG")]
	public static void DebugAssertExists(this FileInfo @this) {
		if (!@this.Exists) throw new Exception($"{@this} is readonly.");
	}

	public static void AssertExists(this FileInfo @this) {
		if (!@this.Exists) throw new Exception($"{@this} is readonly.");
	}

	public static FileInfo WhichExists(this FileInfo @this) {
		if (!@this.Exists) throw new Exception($"{@this} is readonly.");
		return @this;
	}

	#endregion

	#region Int assertion extensions

	[Conditional("DEBUG")]
	public static void DebugAssertIsMultipleOf(this int @this, int multipleSize) {
		if (!@this.IsMultipleOf(multipleSize)) throw new ArgumentException($"{@this} is not a multiple of {multipleSize}: div={@this / multipleSize}, mod={@this % multipleSize}.");
	}

	public static void AssertIsMultipleOf(this int @this, int multipleSize) {
		if (!@this.IsMultipleOf(multipleSize)) throw new ArgumentException($"{@this} is not a multiple of {multipleSize}: div={@this / multipleSize}, mod={@this % multipleSize}.");
	}

	public static int WhichIsMultipleOf(this int @this, int multipleSize) {
		if (!@this.IsMultipleOf(multipleSize)) throw new ArgumentException($"{@this} is not a multiple of {multipleSize}: div={@this / multipleSize}, mod={@this % multipleSize}.");
		return @this;
	}

	#endregion

	#region Long assertion extensions

	[Conditional("DEBUG")]
	public static void DebugAssertIsMultipleOf(this long @this, long multipleSize) {
		if (!@this.IsMultipleOf(multipleSize)) throw new Exception($"{@this} is not a multiple of {multipleSize}: div={@this / multipleSize}, mod={@this % multipleSize}.");
	}

	public static void AssertIsMultipleOf(this long @this, long multipleSize) {
		if (!@this.IsMultipleOf(multipleSize)) throw new Exception($"{@this} is not a multiple of {multipleSize}: div={@this / multipleSize}, mod={@this % multipleSize}.");
	}

	public static long WhichIsMultipleOf(this long @this, long multipleSize) {
		if (!@this.IsMultipleOf(multipleSize)) throw new Exception($"{@this} is not a multiple of {multipleSize}: div={@this / multipleSize}, mod={@this % multipleSize}.");
		return @this;
	}

	#endregion

	#region relational and equality assertions

	public static long MustBeGreaterThan(this long @this, long val, Func<string>? message = null, string? name = null) {
		if (@this > val) return @this;
		throw new Exception(message?.Invoke() ?? $"{name ?? "long"}={@this} is not greater than {val}.");
	}

	public static long MustBeGreaterThan(this long @this, long val, string name) {
		if (@this > val) return @this;
		throw new Exception($"{name ?? "long"}={@this} is not greater than {val}.");
	}

	public static long MustBe(this long @this, long val, Func<string>? message = null, string? name = null) {
		if (@this == val) return @this;
		throw new Exception(message?.Invoke() ?? $"{name ?? "long"}={@this} is not equal to {val}.");
	}

	public static int MustBe(this int @this, int val, Func<string>? message = null, string? name = null) {
		if (@this == val) return @this;
		throw new Exception(message?.Invoke() ?? $"{name ?? "int"}={@this} is not equal to {val}.");
	}

	public static string MustBe(this string @this, string arg, Func<string>? message = null, string? name = null) {
		for (var i = 0; i < @this.Length; ++i) {
			if (arg.Length >= i) {
				throw new Exception(message?.Invoke() ??
				                    $"{name ?? "string"} is longer than {arg.Length}.");
			}
			if (@this[i] != arg[i]) {
				throw new Exception(message?.Invoke() ??
				                    $"Strings differs at index {i}.");
			}
		}
		if (arg.Length > @this.Length) throw new Exception(message?.Invoke() ?? $"{name ?? "string"} is shorter than {arg.Length}.");
		return @this;
	}

	#endregion

	#region String assertion extesions

	public static string WhichIsNotNullOrEmpty(this string @this) {
		if (@this == null) throw new Exception("String is null.");
		if (@this.Length == 0) throw new Exception("String is empty.");
		return @this;
	}

	public static string WhichIsNotNullOrWhiteSpace(this string @this) {
		if (string.IsNullOrWhiteSpace(@this.WhichIsNotNullOrEmpty())) throw new Exception("String is whitespace.");
		return @this;
	}

	public static string MustNotBeEmpty(this string @this, Func<string>? message = null, string? name = null) {
		if (@this == null) throw new Exception(message?.Invoke() ?? $"{name ?? "string"} is null.");
		if (@this.Length == 0) throw new Exception(message?.Invoke() ?? $"{name ?? "string"} is empty.");
		return @this;
	}

	#endregion
	
	public static Uri AssertIsBaseUrl(this Uri @this) => @this.Assert(x => string.IsNullOrEmpty(x.Query), x => $"{x} is not a base url because it has a query part.");
}