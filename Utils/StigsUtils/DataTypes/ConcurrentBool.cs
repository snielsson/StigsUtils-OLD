// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.
namespace StigsUtils.DataTypes;

/// <summary>
///   A boolean providing an atomic (thread safe) test and set operation.
/// </summary>
public struct ConcurrentBool {
	private int _value;
	public bool IsSet => _value != 0;

	public static implicit operator bool(ConcurrentBool x) => x.IsSet;

	public ConcurrentBool(bool initialValue = false) => _value = initialValue ? 1 : 0;

	public bool IsFirstSet => TrySet();

	public bool TrySet(bool val = true) => Interlocked.CompareExchange(ref _value, val ? 1 : 0, val ? 0 : 1) == (val ? 0 : 1);
}