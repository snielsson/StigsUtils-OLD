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
		Assert.True(((string?)null).IsNullOrEmpty());
		Assert.True("".IsNullOrEmpty());
		Assert.False(" ".IsNullOrEmpty());
		Assert.False("\n".IsNullOrEmpty());
		Assert.False("\t".IsNullOrEmpty());
		Assert.False("abc".IsNullOrEmpty());
	}

	[Fact]
	public void IsNullOrWhitespaceWorks() {
		Assert.True(((string?)null).IsNullOrWhiteSpace());
		Assert.True("".IsNullOrWhiteSpace());
		Assert.True(" ".IsNullOrWhiteSpace());
		Assert.True("\n".IsNullOrWhiteSpace());
		Assert.True("\t".IsNullOrWhiteSpace());
		Assert.False("abc".IsNullOrWhiteSpace());
	}

	
	
	[Fact]
	public void MatchesWildCardWorks() {
		"".MatchesWildCard("").Should().BeTrue();
		"abc".MatchesWildCard("abc").Should().BeTrue();
		"abc".MatchesWildCard("ab").Should().BeFalse();
		"ab".MatchesWildCard("abc").Should().BeFalse();
		
		"abc".MatchesWildCard("a?c").Should().BeTrue();
		"abc".MatchesWildCard("??c").Should().BeTrue();
		"abc".MatchesWildCard("???").Should().BeTrue();
		"abc".MatchesWildCard("????").Should().BeFalse("wildcard requires 4 characters.");

		"abcd".MatchesWildCard("a*d").Should().BeTrue();
		"abcxxxd".MatchesWildCard("a*c*d").Should().BeTrue();
		"abcxxxd".MatchesWildCard("a*c*d*").Should().BeTrue();
		"abcxxxd".MatchesWildCard("a*cd").Should().BeFalse();
		"abcxxxd".MatchesWildCard("a*cd*").Should().BeFalse();
		"abcd".MatchesWildCard("abcd*").Should().BeTrue();
		"abcd".MatchesWildCard("abcd***").Should().BeTrue();
		"abcd".MatchesWildCard("***abcd").Should().BeTrue();
		"abcd".MatchesWildCard("*abcd*").Should().BeTrue();
		"abcd".MatchesWildCard("*a*b*c*d*").Should().BeTrue();
		
		string? s = null;

		Action act = () => s!.MatchesWildCard("abc");
		act.Should().Throw<ArgumentNullException>().WithParameterName("this");

		act = () => "".MatchesWildCard(null!);
		act.Should().Throw<ArgumentNullException>().WithParameterName("wildCardPattern");
		
		
	}

	[Fact]
	public void TrimEndWorks() {
		"abc".TrimEnd("").Should().Be("abc");
		"abc".TrimEnd("c").Should().Be("ab");
		"abc".TrimEnd("abc").Should().Be("");
		"abc".TrimEnd("abcd").Should().Be("abc");
	}

	[Fact]
	public void WhiteSpaceSplitWorks() {
		"abc".WhiteSpaceSplit().Should().Equal("abc");
		" abc".WhiteSpaceSplit().Should().Equal("abc");
		"abc ".WhiteSpaceSplit().Should().Equal("abc");
		"a bc".WhiteSpaceSplit().Should().Equal("a","bc");
		" a\tbc".WhiteSpaceSplit().Should().Equal("a","bc");
		"a \nbc ".WhiteSpaceSplit().Should().Equal("a","bc");
	}
}