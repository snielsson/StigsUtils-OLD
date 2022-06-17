// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.
namespace StigsUtils.Extensions;

public static class NullableExtensions {
	public static bool ToBool(this bool? @this) => @this != null && @this.Value;
}