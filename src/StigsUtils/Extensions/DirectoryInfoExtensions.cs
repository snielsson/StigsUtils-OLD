// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
namespace StigsUtils.Extensions;

public static class DirectoryInfoExtensions {
	public static DirectoryInfo Combine(this DirectoryInfo @this, params string[] args) {
		var result = new DirectoryInfo(@this.FullName.Combine(args));
		return result;
	}

	public static FileInfo Combine(this DirectoryInfo @this, FileInfo fileInfo) {
		var result = new FileInfo(@this.FullName.Combine(fileInfo.Name));
		return result;
	}
}