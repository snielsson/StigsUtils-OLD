// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using FluentAssertions;
using StigsUtils.DataTypes;
using Xunit;
namespace StigsUtils.Tests.DataTypes;

public class VersionInfoTests { 
	[Fact]
	public void VersionInfoParseWorks() {
		VersionInfo? sut1 = VersionInfo.Parse("1.2.3+SomeSha");
		sut1.MajorVersion.Should().Be(1);
		sut1.MinorVersion.Should().Be(2);
		sut1.PatchVersion.Should().Be(3);
		sut1.PreReleaseTag.Should().Be("");
		sut1.PreReleaseVersion.Should().Be(null);
		sut1.GitHeight.Should().Be(null);
		sut1.GitCommitSha.Should().Be("SomeSha");
		VersionInfo? sut2 = VersionInfo.Parse("1.2.3-prerelease.4.5+SomeSha");
		sut2.MajorVersion.Should().Be(1);
		sut2.MinorVersion.Should().Be(2);
		sut2.PatchVersion.Should().Be(3);
		sut2.PreReleaseTag.Should().Be("prerelease");
		sut2.PreReleaseVersion.Should().Be(4);
		sut2.GitHeight.Should().Be(5);
		sut2.GitCommitSha.Should().Be("SomeSha");
	}
}