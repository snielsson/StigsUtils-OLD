// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using FluentAssertions;
using StigsUtils.Extensions;
using Xunit;
namespace StigsUtils.Tests.Extensions;

public class StringExtensionsTests {

	[Theory]
	[InlineData(null, null)]
	[InlineData("", null)]
	[InlineData("abc", null)]
	[InlineData("-1", -1)]
	[InlineData("0", 0)]
	[InlineData("1", 1)]
	public void AsIntWorks(string? testValue, int? expectedValue) {
		testValue.AsInt().Should().Be(expectedValue);
	}

	[Fact]
	public void IsNullOrEmptyWorks() {
		Assert.True(((string)null).IsNullOrEmpty());
		Assert.True("".IsNullOrEmpty());
		Assert.False(" ".IsNullOrEmpty());
		Assert.False("\n".IsNullOrEmpty());
		Assert.False("\t".IsNullOrEmpty());
		Assert.False("abc".IsNullOrEmpty());
	}

	[Fact]
	public void IsNullOrWhitespaceWorks() {
		Assert.True(((string)null).IsNullOrWhiteSpace());
		Assert.True("".IsNullOrWhiteSpace());
		Assert.True(" ".IsNullOrWhiteSpace());
		Assert.True("\n".IsNullOrWhiteSpace());
		Assert.True("\t".IsNullOrWhiteSpace());
		Assert.False("abc".IsNullOrWhiteSpace());
	}

	[Fact]
	public void TrimEndWorks() {
		"abc".TrimEnd("").Should().Be("abc");
		"abc".TrimEnd("c").Should().Be("ab");
		"abc".TrimEnd("abc").Should().Be("");
		"abc".TrimEnd("abcd").Should().Be("abc");
	}
}