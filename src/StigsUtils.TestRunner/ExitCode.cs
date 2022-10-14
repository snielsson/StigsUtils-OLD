// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
namespace StigsUtils.TestRunner;

public class ExitCode {
	public static ExitCode Ok = new(0);
	private ExitCode(int i) {
		Value = i;
	}
		public static implicit operator int(ExitCode x) => x.Value;
	public int Value { get; }
}