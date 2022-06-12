// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.
namespace StigsUtils.Extensions;

public static class GenericExtensions
{
	public static T[] AsArray<T>(this T @this) => new[] {@this};
	public static T NotNull<T>(this T @this, string? name = null) => @this ?? throw new ArgumentNullException(name);
}