// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System.Runtime.CompilerServices;
namespace StigsUtils.Extensions;

public static class DoubleExtensions {
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsZero(this double @this, double? tolerance = null) => Math.Abs(@this) < (tolerance ?? double.Epsilon);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsEqualTo(this double @this, double comparand, double? tolerance = null) => (@this - comparand).IsZero(tolerance);

	public static bool HasFraction(this double @this, double? tolerance = null) => Math.Abs(@this % 1) > tolerance;
}