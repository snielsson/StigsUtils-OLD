// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
namespace StigsUtils.DataTypes.ValueObjects;

public record Text32 : Text {
	public Text32(string val, int minLength = 0) : base(val, 32, minLength) { }
	public static implicit operator string(Text32 x) => x.Val;
	public static implicit operator Text32(string x) => new(x);
}