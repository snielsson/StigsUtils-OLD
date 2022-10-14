// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.
namespace StigsUtils.Extensions;

public static class NumericExtensions {
	public static bool IsEven(this int @this) => @this % 2 == 0;

	public static bool IsOdd(this int @this) => @this % 2 == 1;
}