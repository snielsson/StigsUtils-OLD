// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.
using System.Diagnostics;
using StigsUtils.DataTypes.Exceptions;
namespace StigsUtils.Extensions;

public static class ExceptionExtensions
{
	public static void Throw<TThrower, TValue>(this TThrower @this, TValue value = default!)
	{
		throw new Error<TThrower, TValue>(@this, value);
	}

	public static void Throw<TThrower, TValue>(this TThrower @this, string? message, TValue value = default!)
	{
		throw new Error<TThrower, TValue>(@this, message, value);
	}
}