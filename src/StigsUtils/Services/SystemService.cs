// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.
using System.Diagnostics;
using StigsUtils.TimeUtils;
namespace StigsUtils.Services;

public interface ISystemService {
	UtcDateTime UtcNow { get; }
}
public class SystemService : ISystemService {
	private UtcDateTime? _utcNow;
	public UtcDateTime UtcNow {
		get => _utcNow ?? DateTime.UtcNow;
		set => _utcNow = value;
	}

	private static bool OpenWithDefaultProgram(string path)
		=> new Process {
			StartInfo = {
				FileName = "explorer",
				Arguments = "\"" + path + "\""
			}
		}.Start();
}

