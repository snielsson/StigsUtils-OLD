// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.
namespace StigsUtils.DataTypes.Comparers;

public class NeverEqualComparer : IComparer<decimal> {
	public int Compare(decimal x, decimal y) => x.CompareTo(y) >= 0 ? 1 : -1;
}