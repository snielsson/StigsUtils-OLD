// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
namespace StigsUtils.Extensions;

public static class UriExtensions
{
	public static Uri AssertIsBaseUrl(this Uri @this) => @this.Assert(x => string.IsNullOrEmpty(x.Query), x => $"{x} is not a base url because it has a query part.");
}