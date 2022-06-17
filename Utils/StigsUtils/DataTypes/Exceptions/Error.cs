// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
namespace StigsUtils.DataTypes.Exceptions;


public class Error<T> : Exception {
	public Error(T thrower) => Thrower = thrower;

	protected Error(T thrower, SerializationInfo info, StreamingContext context) : base(info, context) => Thrower = thrower;

	public Error(T thrower, string? message) : base(message) => Thrower = thrower;

	public Error(T thrower, string? message, Exception? innerException) : base(message, innerException) => Thrower = thrower;

	public T Thrower { get; protected set; }
	public string? CallerName { get; internal set; }
	public string? CallerFilePath { get; internal set; }
	public int CallerLineNumber { get; internal set; }
}

public class Error<TThrower, TValue> : Error<TThrower>
{
	public Error(TThrower thrower, string? message, TValue value = default!) : base(thrower, message)
	{
		Thrower = thrower;
		Value = value;
	}

	public Error(TThrower thrower, TValue value = default!) : base(thrower)
	{
		Thrower = thrower;
		Value = value;
	}
	public TValue Value { get; }
}

public static class ErrorExtensions {
	public static Error<T> Error<T>(this T @this, string? message = null, [CallerMemberName] string? callerName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0) => new Error<T>(@this, message) {
		CallerName = callerName,
		CallerFilePath = callerFilePath,
		CallerLineNumber = callerLineNumber
	};
}