// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using FluentAssertions;
using StigsUtils.BuildUtils;
using StigsUtils.DataTypes.Exceptions;
using StigsUtils.FileUtils;
using Xunit;
namespace StigsUtils.Tests.BuildUtils;

public class SolutionTests	 {
	[Fact]
	public void GetBinariesThrowsErrorForNonExistingSolutionFile() {
		Action act = () => SolutionFileParser.GetBinaries(new FilePath("non-existing file"), "", "").ToArray(); 
		act.Should()
			.Throw<Error<FilePath>>()
			.WithMessage("Solution file * does not exist.");
	}
	
	[Fact]
	public void GetBinariesThrowsArgumentExceptionForInvalidContent() {
		Action act = () => SolutionFileParser.GetBinaries("invalid content", "", "").ToArray(); 
		act.Should()
			.Throw<ArgumentException>()
			.WithParameterName("solutionFileContent")
			.WithMessage("Could not parse*");
	}

	[Fact]
	public void GetDebugBinariesWorksForValidFile() {
		SolutionFileParser.GetBinaries(new FilePath(@"C:\projects\StigsUtils\StigsUtils.sln"), "Debug","").Should().HaveCount(15);
	}
	
	[Fact]
	public void GetDebugBinariesWorksForValidContent() {
		var content = Resource.ReadAllText("TestData.TestSolutionFile.txt");
		SolutionFileParser.GetBinaries(content, "Debug","").Should().HaveCount(15);
	}
}