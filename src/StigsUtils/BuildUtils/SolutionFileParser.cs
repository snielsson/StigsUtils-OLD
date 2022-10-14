// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System.Text;
using System.Text.RegularExpressions;
using StigsUtils.FileUtils;
namespace StigsUtils.BuildUtils;

public class StreamParser {
	private readonly StreamReader _reader;
	public StreamParser(StreamReader reader) {
		_reader = reader;
	}

	public StreamParser SkipPast(string s) {
		if (_reader.EndOfStream) return this;
		var i = 0;
		int next;
		while ((next = _reader.Read()) > -1) {
			if (next == s[i]) {
				i++;
				if (i == s.Length) break;
			}
			else i = 0;
		}
		return this;
	}

	public StreamParser ReadPast(string s, out string? result) {
		result = null;
		if (_reader.EndOfStream) return this;
		var sb = new StringBuilder();
		var i = 0;
		int next;
		while ((next = _reader.Read()) > -1) {
			sb.Append((char)next);
			if (next == s[i]) {
				i++;
				if (i == s.Length) break;
			}
			else i = 0;
		}
		//result = sb. 
		return this;
	}
}


public sealed class SolutionFileParser {
	public static IEnumerable<FilePath> GetBinaries(FilePath solutionFilePath, string buildType, string projectName) {
		return GetBinaries(solutionFilePath.AssertExists($"Solution file {solutionFilePath} does not exist.").ReadAllText(), buildType, projectName);
	}

	//Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "StigsUtils", "src\StigsUtils\StigsUtils.csproj", "{F362A974-52CA-4D0A-96A9-2612AF1BBA86}"
//	EndProject


	private static readonly Regex _projectRegex = 
		new(@"^Project\(""(?'TypeGuid'[^""])+""\) = ""(?'Name'[^""])+"", ""(?'RelPath'[^""])+"", ""(?'ProjectGuid'[^""]+)""$", 
			RegexOptions.Compiled);

	public readonly record struct Project(Guid Type, string Name, string RelPath, Guid ProjectGuid); 
	
	public static IEnumerable<Project> GetProjects(string solutionFileContent) {
		Project result = default;
		foreach (Match match in _projectRegex.Matches(solutionFileContent)) {
			try {
				var typeGuid = Guid.Parse(match.Groups["TypeGuid"].Value);
				var name = match.Groups["Name"].Value;
				var relPath = match.Groups["RelPath"].Value;
				var projectGuid = Guid.Parse(match.Groups["ProjectGuid"].Value);
				result = new Project(typeGuid, name, relPath, projectGuid);
			}
			catch (Exception ex) {
				throw new ArgumentException($"Could not parse string content as a solution file: {ex.Message}", nameof(solutionFileContent), ex);
			}
			yield return result;
		}
	}
	public static IEnumerable<FilePath> GetBinaries(string solutionFileContent, string buildType, string projectName, FilePath? filePath = null) {
		// try {
		// 	SolutionFile? solutionFile = SolutionFile.Parse(solutionFileContent) ?? throw new ArgumentException($"Could not parse {filePath ?? "string"} as a solution file.",nameof(solutionFileContent));
		// }
		// catch (Exception ex) { 
		// 	throw new ArgumentException($"Could not parse {filePath ?? "string"} as a solution file: {ex.Message}", nameof(solutionFileContent),ex);
		// }
		
		
		
		yield break;
	}

	/*
	 *
	 * 	public static void Run(FilePath solutionFilePath, string buildType) {
	if (solutionFilePath == null) throw new ArgumentNullException(nameof(solutionFilePath));
	SolutionFile? solutionFile = SolutionFile.Parse(solutionFilePath) ?? throw new ArgumentException($"Could not parse {solutionFilePath} as a solution file.");
	foreach (ProjectInSolution projectInSolution in solutionFile.ProjectsInOrder) {
		var project = ProjectRootElement.Open(projectInSolution.AbsolutePath);
		string targetFramework = project.Properties.SingleOrDefault(x => x.Name == "TargetFramework")?.Value ?? throw new Exception("Could not determine bin dir for " + project.FullPath);
		var projectName = Path.GetFileNameWithoutExtension(projectInSolution.AbsolutePath);
		var assemblyName = project.Properties.SingleOrDefault(x => x.Name == "AssemblyName")?.Value ?? projectName;
		var assemblyFilePath = Path.Combine(project.DirectoryPath, "bin",buildType, targetFramework,assemblyName+".dll");		
		bool e= File.Exists(assemblyFilePath);
		Console.WriteLine(assemblyFilePath);
	}
	

	 * 
	 */
}