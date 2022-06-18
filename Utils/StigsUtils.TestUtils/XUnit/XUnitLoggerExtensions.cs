// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StigsUtils.LoggingUtils;
using Xunit.Abstractions;

namespace StigsUtils.TestUtils.XUnit;

public static class XUnitLoggerExtensions
{
    public static IHostBuilder AddXUnitLogging(this IHostBuilder @this, ITestOutputHelper output) =>
        @this.ConfigureLogging(x => x.Services.AddXUnitLogging(output));

    public static IServiceCollection AddXUnitLogging(this IServiceCollection @this, ITestOutputHelper output)
    {
        var xunitLoggerProvider = new XUnitLoggerProvider(output);
        return @this
            .AddSingleton<ILoggerProvider>(xunitLoggerProvider)
            .AddSingleton<ILogHistory>(xunitLoggerProvider);
    }
}