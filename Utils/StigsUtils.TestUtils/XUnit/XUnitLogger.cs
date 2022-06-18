// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.

using System.Text;
using Microsoft.Extensions.Logging;
using StigsUtils.LoggingUtils;
using Xunit.Abstractions;

namespace StigsUtils.TestUtils.XUnit;

/// <summary>
///     Inspired by https://www.meziantou.net/how-to-get-asp-net-core-logs-in-the-output-of-xunit-tests.htm,
/// </summary>
internal class XUnitLogger : ILogger
{
    private readonly string? _categoryName;
    private readonly LoggerExternalScopeProvider _scopeProvider;
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly XUnitLoggerProvider _xUnitLoggerProvider;

    public XUnitLogger(XUnitLoggerProvider xUnitLoggerProvider, ITestOutputHelper testOutputHelper, LoggerExternalScopeProvider scopeProvider, string? categoryName)
    {
        _xUnitLoggerProvider = xUnitLoggerProvider;
        _testOutputHelper = testOutputHelper;
        _scopeProvider = scopeProvider;
        _categoryName = categoryName;
    }

    public IEnumerable<LogHistoryEntry> LogEntries => _xUnitLoggerProvider.LogEntries;

    public bool IsEnabled(LogLevel logLevel) =>
        logLevel != LogLevel.None;

    public IDisposable BeginScope<TState>(TState state) =>
        _scopeProvider.Push(state);

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        StringBuilder stringBuilder = new StringBuilder()
                .Append($"{GetLogLevelString(logLevel)}:")
                .Append(DateTime.Now.ToString("hh:mm:ss.fff: "))
                .Append(formatter(state, exception))
                .Append("(").Append(_categoryName).Append(")")
                .Append(exception != null ? $"\n{exception}" : "")
            ;

        // Append scopes
        _scopeProvider.ForEachScope(
            (scope, localStringBuilder) =>
            {
                localStringBuilder.Append("\n => ");
                localStringBuilder.Append(scope);
            },
            stringBuilder);

        _testOutputHelper.WriteLine(stringBuilder.ToString());

        _xUnitLoggerProvider.AddToHistory(
            new LogHistoryEntry
            {
                LogLevel = logLevel,
                EventId = eventId,
                Message = formatter(state, exception!),
                Exception = exception
            });
    }

    public static XUnitLogger Create<T>(ITestOutputHelper output) => new(new XUnitLoggerProvider(output), output, new LoggerExternalScopeProvider(), typeof(T).Name);

    private static string GetLogLevelString(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Trace => "T",
            LogLevel.Debug => "D",
            LogLevel.Information => "INFO",
            LogLevel.Warning => "W",
            LogLevel.Error => "E",
            LogLevel.Critical => "C",
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel))
        };
    }
}

internal sealed class XUnitLogger<T> : XUnitLogger, ILogger<T>
{
    public XUnitLogger(XUnitLoggerProvider loggerProvider, ITestOutputHelper testOutputHelper, LoggerExternalScopeProvider scopeProvider) : base(loggerProvider, testOutputHelper, scopeProvider,
        typeof(T).FullName)
    {
    }
}