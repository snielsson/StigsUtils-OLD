// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
namespace StigsUtils.DataTypes;

public static class MinMax {
	public static T Max<T>(T x, T y) where T : IComparable<T> => x.CompareTo(y) == -1 ? y : x;
	public static T Min<T>(T x, T y) where T : IComparable<T> => x.CompareTo(y) == 1 ? y : x;
	public static T Min<T>(params T[] args) where T : IComparable<T> {
		T? min = args[0];
		if (args.Length > 1) {
			for (var i = 1; i < args.Length; i++) {
				if (min.CompareTo(args[i]) == -1) min = args[i];
			}
		}
		return min;
	}
	public static T Max<T>(params T[] args) where T : IComparable<T> {
		T? min = args[0];
		if (args.Length > 1) {
			for (var i = 1; i < args.Length; i++) {
				if (min.CompareTo(args[i]) == 1) min = args[i];
			}
		}
		return min;
	}
        
}