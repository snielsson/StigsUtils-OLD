// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
namespace StigsUtils.DataTypes;

public record TimeInterval : Interval<DateTime> {
	public TimeInterval(DateTime start, DateTime end) : base(start, end) { }
}