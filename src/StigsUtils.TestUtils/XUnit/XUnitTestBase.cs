// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StigsUtils.Extensions;
using Xunit.Abstractions;

namespace StigsUtils.TestUtils.XUnit;

public abstract class XUnitTestBase : IDisposable
{
    private const string DefaultEnvironment = "Local";
    private readonly Lazy<IHost> _host;
    private readonly ITestOutputHelper _output;
    private readonly DateTime _startTime;
    public readonly IHostBuilder Builder;

    protected XUnitTestBase(ITestOutputHelper output, string environment = DefaultEnvironment) : this(output, environment, null)
    {
    }

    protected XUnitTestBase(ITestOutputHelper output, IHostBuilder builder) : this(output, DefaultEnvironment, builder)
    {
    }

    private XUnitTestBase(ITestOutputHelper output, string environment, IHostBuilder? builder)
    {
        _output = output;
        _output.WriteLine($"Test started at  {_startTime = DateTime.Now:O}:\n");
        Builder = (builder ?? Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(new[] { $"--environment={environment}" }))
            .ConfigureAppConfiguration((ctx, configuration) => { configuration.SetValue("Logging:LogLevel:Default", "Trace"); })
            .ConfigureServices(services => services.AddSingleton<ILoggerProvider>(_ => new XUnitLoggerProvider(output)));
        _host = new Lazy<IHost>(() => Builder.Build());
    }

    protected IHost Host => _host.Value;
    protected IServiceProvider ServiceProvider => Host.Services;
    protected ILogger Logger => Get<ILogger<XUnitTestBase>>();

    public virtual void Dispose()
    {
        DateTime now = DateTime.Now;
        _output.WriteLine($"\nTest finished at {DateTime.Now:O}, ran for {(now - _startTime).TotalMilliseconds}ms.");
    }

    protected T Get<T>() where T : notnull => ServiceProvider.GetRequiredService<T>();
}