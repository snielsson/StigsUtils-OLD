// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System;
using System.Collections.Generic;
using Xunit;
namespace StigsUtils.Tests.MiscTests; 

public class MiscExperiments {
	[Fact]
	public void NAME() {
		var t1 = DateTime.Parse("2022-06-22T15:43:45.3094204+00:00");
		var t2 = DateTime.Parse("2022-06-22T15:43:46.1300168+00:00");
		
		var diff = t2 - t1;
	}
}

public class StrictComparer<T> : IComparer<T> where T:IComparable<T>
{
	public static readonly StrictComparer<T> Default = new StrictComparer<T>();
	public int Compare(T x, T y)
	{
		if (x == null) throw new ArgumentNullException(nameof(x));
		var result = x.CompareTo(y);
		if (result == 0) return -1;
		return result; 
	}
}

public class SortedBag {

}