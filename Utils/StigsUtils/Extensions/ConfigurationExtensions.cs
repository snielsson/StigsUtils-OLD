// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.

using Microsoft.Extensions.Configuration;

namespace StigsUtils.Extensions;

public static class ConfigurationExtensions
{
    public static IConfigurationBuilder SetValue(this IConfigurationBuilder @this, string key, string value) =>
        @this.AddInMemoryCollection(new[] { new KeyValuePair<string, string>(key, value) });
}