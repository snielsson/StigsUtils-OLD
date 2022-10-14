// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System.ComponentModel.DataAnnotations;
namespace StigsUtils.DataTypes.ValueObjects;

public record Text {
	public Text(string val, int maxLength = int.MaxValue, int minLength = 0) {
		if (val.Length > maxLength) throw new ValidationException($"{GetType()} has max length {maxLength} but was initialized with string with length {val.Length}: {val}.");
		if (val.Length < minLength) throw new ValidationException($"{GetType()} has min length {minLength} but was initialized with string with length {val.Length}: {val}.");
		Val = val;
	}

	public string Val { get; }

	public static implicit operator string(Text x) => x.Val;

	public static implicit operator Text(string x) => new(x);

	public override string ToString() => Val;
}