// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using FluentAssertions;
using StigsUtils.Extensions;
using Xunit;
namespace StigsUtils.Tests.Extensions;

public class StreamReaderExtensionsTests {
	[Fact]
	public void ReadAndSkipPastWorks() {
		using var sut = "ab x=y cd x=z ef".ToStreamReader();
		sut.SkipPast("ab ").Read(3).Should().Be("x=y");
		sut.SkipPast(" cd ").Read(2).Should().Be("x=");
		sut.Read("e").Should().Be("e");
		sut.Read("e").Should().BeNull();
		sut.EndOfStream.Should().BeFalse();
		sut.Read("f").Should().Be("f");
		sut.EndOfStream.Should().BeTrue();
		sut.Read("f").Should().BeNull();
	}
}