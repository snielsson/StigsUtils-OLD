// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.
namespace StigsUtils.DataTypes.Comparers;

public class ThrowOnEqualComparer : IComparer<decimal> {
	public static ThrowOnEqualComparer Default { get; } = new();

	public int Compare(decimal x, decimal y) {
		var result = x.CompareTo(y);
		if (result == 0) throw new Exception("Equal value " + x);
		return x.CompareTo(y) >= 0 ? 1 : -1;
	}
}