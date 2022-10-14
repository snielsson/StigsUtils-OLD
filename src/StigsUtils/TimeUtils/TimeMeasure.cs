// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System.Diagnostics;
namespace StigsUtils.TimeUtils;

class TimeMeasure {
	private readonly long _start;
	public TimeMeasure() {
		_start = Stopwatch.GetTimestamp();
	}
	public double ElapsedMs => Math.Round((double)(Stopwatch.GetTimestamp() - _start) / Stopwatch.Frequency * 1000,3);
}