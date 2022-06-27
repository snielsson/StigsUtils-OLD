// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System.Reflection;
using System.Text.RegularExpressions;
using StigsUtils.Extensions;
namespace StigsUtils.DataTypes;

public class VersionInfo {
	public int MajorVersion { get; set; }
	public int MinorVersion { get; set; }
	public int PatchVersion { get; set; }
	public string? PreReleaseTag { get; set; }
	public int? PreReleaseVersion { get; set; }
	public int? GitHeight { get; set; }
	public string? GitCommitSha { get; set; }
	private static Regex _parseRegex = new Regex(@"^(?<Major>\d+)\.(?<Minor>\d+)\.(?<Patch>\d+)(?:-(?<PreTag>[^.]*)\.(?<PreVersion>\d+)\.(?<GitHeight>\d+))?\+(?<GitCommitSha>.*)$", RegexOptions.Compiled);
	public static VersionInfo? Parse(string s) {
		var match = _parseRegex.Match(s);
		if (match.Success) {
			return new VersionInfo { 
				MajorVersion = match.Groups["Major"].Value.ToInt(),
				MinorVersion = match.Groups["Minor"].Value.ToInt(),
				PatchVersion = match.Groups["Patch"].Value.ToInt(),
				PreReleaseTag = match.Groups["PreTag"].Value,
				PreReleaseVersion = match.Groups["PreVersion"].Value.AsInt(),
				GitHeight = match.Groups["GitHeight"].Value.AsInt(),
				GitCommitSha = match.Groups["GitCommitSha"].Value
			};
		}
		return default;
	}
	public static VersionInfo? Parse(Assembly a) => a.GetVersionInfo();
}