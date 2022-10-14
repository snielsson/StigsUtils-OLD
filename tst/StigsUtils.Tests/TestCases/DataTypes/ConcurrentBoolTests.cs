// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.
using FluentAssertions;
using StigsUtils.DataTypes;
using Xunit;
namespace StigsUtils.Tests.DataTypes;

public class ConcurrentBoolTests {
	[Fact]
	public void FlagWorks() {
		var target = new ConcurrentBool();
		((bool)target).Should().Be(false);
		target.TrySet().Should().BeTrue();
		target.TrySet().Should().BeFalse();
		target.TrySet(false).Should().BeTrue();
		target.TrySet(false).Should().BeFalse();
		target.TrySet().Should().BeTrue();
		target.TrySet(false).Should().BeTrue();

		target = new ConcurrentBool(true);
		((bool)target).Should().Be(true);
		target.TrySet().Should().BeFalse();
		target.TrySet(false).Should().BeTrue();
		target.TrySet(false).Should().BeFalse();
		target.TrySet().Should().BeTrue();
		target.TrySet(false).Should().BeTrue();
		target.TrySet().Should().BeTrue();
		target.TrySet(false).Should().BeTrue();

		target = new ConcurrentBool();
		target.IsFirstSet.Should().BeTrue();

		target = new ConcurrentBool(true);
		target.IsFirstSet.Should().BeFalse();

	}
}