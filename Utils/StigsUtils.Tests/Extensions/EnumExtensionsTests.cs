// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using StigsUtils.Extensions;
using Xunit;
namespace StigsUtils.Tests.Extensions;

public class EnumExtensionsTests {
	
	public enum EnumWithConsecutiveValues {
		None,
		Val1,
		Val2
	}
	public enum EnumWithNonConsecutiveValues : long {
		None,
		Val1 = 3,
		Val2
	}
	[Fact]
	public void IsEnumWithConsecutiveValuesWorks() {
		Assert.False(typeof(int).IsEnumWithConsecutiveValues());
	}

	//TODO: fix this test (easy)
	// [Fact]
	// public void AssertIsConsecutiveWorks() {
	// 	Should()..NotThrow(() => typeof(EnumWithConsecutiveValues).AssertIsEnumWithConsecutiveValues());
	// 	Should.Throw<Exception>(() => typeof(EnumWithNonConsecutiveValues).AssertIsEnumWithConsecutiveValues());
	// }
}