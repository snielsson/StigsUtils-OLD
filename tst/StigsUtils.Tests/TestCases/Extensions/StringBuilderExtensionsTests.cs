// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System.Text;
using FluentAssertions;
using StigsUtils.Extensions;
using Xunit;
namespace StigsUtils.Tests.Extensions;

public class StringBuilderExtensionsTests
{
	[Fact]
	public void EndsWithWorks() {
		var sb = new StringBuilder("abcdef");
		Assert.True(sb.EndsWith("f"));
		Assert.True(sb.EndsWith("abcdef"));
		Assert.False(sb.EndsWith("a"));
		Assert.False(sb.EndsWith("aabcdef"));
		Assert.False(sb.EndsWith(""));
		Assert.False(sb.EndsWith(null!));
	}
	
	[Fact]
	public void LastOrDefaultWorks() {
		new StringBuilder("abc").LastOrDefault().Should().Be('c');
		new StringBuilder("").LastOrDefault().Should().Be(default);
	}

	[Theory]
	[InlineData("")]
	[InlineData("abc")]
	public void FlushWorks(string x) {
		StringBuilder sut = new StringBuilder(x);
		sut.Length.Should().Be(x.Length);
		sut.Flush().Should().Be(x);
		sut.Length.Should().Be(0);
	}

	[Theory]
	[InlineData("")]
	[InlineData("abc")]
	public void FlushToOutStringWorks(string x) {
		var sut = new StringBuilder(x);
		sut.Length.Should().Be(x.Length);
		sut.FlushTo(out string result).Length.Should().Be(0);
		result.Should().Be(x);
	}

	[Theory]
	[InlineData("")]
	[InlineData("abc")]
	public void FlushToTextWriterWorks(string x) {
		var sut = new StringBuilder(x);
		var result = new StringBuilder();
		sut.Length.Should().Be(x.Length);
		sut.FlushTo(new StringWriter(result)).Length.Should().Be(0);
		result.ToString().Should().Be(x);
	}
	
	
	[Theory]
	[InlineData("abcdef",-1,null)]
	[InlineData("abcdef",0,"abcdef")]
	[InlineData("abcdef",1,"abcde")]
	[InlineData("abcdef",3,"abc")]
	[InlineData("abcdef",6,"")]
	[InlineData("abcdef",7,"")]
	[InlineData("abcdef",int.MaxValue,"")]
	public void TruncateWorks(string input, int count, string? expected) {
		Func<string> act = () => new StringBuilder(input).Truncate(count).ToString();
		if(expected != null) act().Should().Be(expected); 
		else act.Should().Throw<ArgumentException>().WithParameterName("value");
	}
	
}