// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.
using System.Linq;
using FluentAssertions;
using StigsUtils.DataTypes.Collections;
using Xunit;
namespace StigsUtils.Tests.DataTypes.Collections;

public class RingBufferTests {

	[Fact]
	public void RingBufferWorks() {
		var target = new RingBuffer<int>(3) {
			1,
			2,
			3,
			4
		};

		var array = target.ToArray();

		array.Should().BeEquivalentTo(new[] { 2, 3, 4 });
		target[0].Should().Be(2);
		target[1].Should().Be(3);
		target[2].Should().Be(4);
	}
}