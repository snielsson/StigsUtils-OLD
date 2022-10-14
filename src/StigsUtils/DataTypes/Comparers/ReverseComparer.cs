// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
namespace StigsUtils.DataTypes.Comparers;

public class ReverseComparer<T> : IComparer<T> {
	public static readonly ReverseComparer<T> Default = new ReverseComparer<T>();
	private readonly Func<T, T, int>? _compareFunc;
	public ReverseComparer() { }
	public ReverseComparer(IComparer<T> comparer) {
		_compareFunc = (x, y) => comparer.Compare(y, x);
	}
	public int Compare(T? x, T? y) => _compareFunc?.Invoke(x!, y!) ?? Comparer<T>.Default.Compare(y, x);
}