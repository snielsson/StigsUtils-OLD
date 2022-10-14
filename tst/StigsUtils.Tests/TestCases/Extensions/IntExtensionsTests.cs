// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System;
using FluentAssertions;
using StigsUtils.Extensions;
using Xunit;
namespace StigsUtils.Tests.Extensions;

public class IntExtensionsTests {
	[Fact]
	public void ToMultiplesOf() {
		0.ToMultiplesOf(4).Should().Be(0);
		16.ToMultiplesOf(4).Should().Be(4);
		16.ToMultiplesOf(-4).Should().Be(4);
		(-16).ToMultiplesOf(4).Should().Be(4);
		(-16).ToMultiplesOf(-4).Should().Be(4);
		17.Invoking(x => x.ToMultiplesOf(4)).Should().Throw<ArgumentException>();
	}
	[Fact]
	public void IsMultipleOfWorks() {
		16.IsMultipleOf(4).Should().BeTrue();
		17.IsMultipleOf(4).Should().BeFalse();
	}

}