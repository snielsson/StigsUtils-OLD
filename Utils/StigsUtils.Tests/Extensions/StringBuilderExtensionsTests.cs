// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System.Text;
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
}