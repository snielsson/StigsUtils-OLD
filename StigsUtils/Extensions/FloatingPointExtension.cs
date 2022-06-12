// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.
namespace StigsUtils.Extensions;

public static class FloatingPointExtension
{
	public static double Round(this double @this, int digits = 2, MidpointRounding midpointRounding = MidpointRounding.ToEven) => Math.Round(@this, digits, midpointRounding);
	public static double? Round(this double? @this, int digits = 2, MidpointRounding midpointRounding = MidpointRounding.ToEven)
	{
		if (@this == null) return null;
		return Math.Round(@this.Value, digits, midpointRounding);
	}
}