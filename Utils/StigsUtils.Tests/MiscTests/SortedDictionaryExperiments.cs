// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;
namespace StigsUtils.Tests.MiscTests;

public class SortedDictionaryExperiments {
	[Fact]
	public void CannotHaveDuplicateKeys() {
		var target = new SortedDictionary<int, int>();
		Assert.Throws<ArgumentException>(() => {
			target.Add(1, 1);
			target.Add(1, 2);
		});
	}

	[Fact]
	public void CanRemoveItemsWhileIterating() {
		var target = new SortedDictionary<int, int>();
		target.Add(1, 1);
		target.Add(2, 2);
		target.Add(3, 3);
		target.Add(4, 4);
		target.Add(5, 5);
		target.Add(6, 6);
		target.Add(7, 7);
		target.Count.Should().Be(7);
		foreach (var (key, val) in target) {
			if (key % 2 == 1) target.Remove(key);
		}
		target.Count.Should().Be(3);
		//target.Keys.Should().BeEquivalentTo<int>(new int[][2,4,6]);
	}
}