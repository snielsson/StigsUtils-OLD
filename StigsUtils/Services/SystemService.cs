// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.
using System.Diagnostics;
namespace StigsUtils.Services;

public class SystemService {
	private static bool OpenWithDefaultProgram(string path)
		=> new Process {
			StartInfo = {
				FileName = "explorer",
				Arguments = "\"" + path + "\""
			}
		}.Start();
}