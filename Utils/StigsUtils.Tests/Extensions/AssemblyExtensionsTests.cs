// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System.Reflection;
using FluentAssertions;
using StigsUtils.Extensions;
using Xunit;
using AssemblyExtensions = StigsUtils.Extensions.AssemblyExtensions;
namespace StigsUtils.Tests.Extensions;

public class AssemblyExtensionsTests {
	[Fact(Skip="GetVersionsFromAttributesWorks - depends on some git tag have been set...")]
	public void GetVersionsFromAttributesWorks() {
		var assembly = Assembly.GetAssembly(typeof(AssemblyExtensions));
		var assemblyVersion = assembly.GetAssemblyVersion().Should().Be("","This test will fail and must be updated  when first annotated git tag with a version pattern (major.minor.patch) has been set.");
		var informationalVersion = assembly.GetInformationalVersion().Should().MatchRegex(@"^\d+\.\d+\.\d+-[^.]*\.\d+$");;
		var fileVersion = assembly.GetFileVersion().Should().MatchRegex(@"^\d+\.\d+\.\d+\.\d+$");
	}
}