// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.
using System;
using System.Collections.Generic;
using FluentAssertions;
using StigsUtils.Extensions;
using Xunit;
namespace StigsUtils.Tests.Extensions;

public class TypeExtensionsTests {
	[Fact]
	public void IsOfGenericTypeWorks() {
		typeof(IEnumerable<int>).IsOfGenericType(typeof(IEnumerable<>)).Should().BeTrue();
		typeof(List<int>).IsOfGenericType(typeof(IEnumerable<>)).Should().BeTrue();
		typeof(int[]).IsOfGenericType(typeof(IEnumerable<>)).Should().BeTrue();
		typeof(List<int>).IsOfGenericType(typeof(IList<>)).Should().BeTrue();
		typeof(IEnumerable<int>).IsOfGenericType(typeof(IList<>)).Should().BeFalse();
		typeof(List<int>).IsOfGenericType(typeof(IEnumerable<>), out Type? concreteType).Should().BeTrue();
		concreteType!.Should().Be(typeof(IEnumerable<int>));
	}
}