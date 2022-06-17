// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using FluentAssertions;
using StigsUtils.Extensions;
using StigsUtils.TestUtils.XUnit;
using Xunit;
using Xunit.Abstractions;
namespace StigsUtils.Tests.Extensions;

public class ObjectExtensionsTests : XUnitTestBase {
	public ObjectExtensionsTests(ITestOutputHelper output) : base(output) { }

	public class SomeClass {
		public bool SomePublicBoolProperty { get; set; }
		public int SomePublicIntProperty { get; set; }
		public string? SomePublicStringProperty { get; set; }
		private int SomePrivateIntProperty { get; set; }
		private string? SomePrivateStringProperty { get; set; }
		public int GetSomePrivateIntProperty() => SomePrivateIntProperty;
	}
	
	[Fact]
	public void SetPropertyWorks() {
		var target = new SomeClass();
		target.SetProperty("SomePublicBoolProperty", true);
		target.SomePublicBoolProperty.Should().Be(true);
		target.SetProperty("SomePublicBoolProperty", false);
		target.SomePublicBoolProperty.Should().Be(false);
		target.SetProperty<SomeClass, int>("SomePublicIntProperty", 117);
		target.SomePublicIntProperty.Should().Be(117);
		target.SetProperty<SomeClass, int>("SomePublicIntProperty", 118);
		target.SomePublicIntProperty.Should().Be(118);
		target.SetProperty("SomePublicStringProperty", "117");
		target.SomePublicStringProperty.Should().Be("117");
	}
}