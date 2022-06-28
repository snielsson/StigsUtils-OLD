// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using FluentAssertions;
using Xunit;
namespace StigsUtils.Tests.MiscTests;

public class SortedListExperiments {

	[Fact]
	public void CanHaveDuplicateKeysWhenUsingStrictComparer() {
		var target = new SortedList<int, int>(StrictComparer<int>.Default);
		target.Add(1, 1);
		target.Add(1, 2);
		target.Count.Should().Be(2, "2 items added and strict comparer used.");
		KeyValuePair<int, int>[] items = target.ToArray();
		items[0].Value.Should().Be(1);
		items[1].Value.Should().Be(2);
	}

	[Fact]
	public void CannotHaveDuplicateKeys() {
		var target = new SortedList<int, int>();
		Assert.Throws<ArgumentException>(() => {
			target.Add(1, 1);
			target.Add(1, 2);
		});
	}
}