// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.

using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using StigsUtils.LoggingUtils;
using Xunit.Abstractions;

namespace StigsUtils.TestUtils.XUnit;

internal sealed class XUnitLoggerProvider : ILoggerProvider, ILogHistory
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly LoggerExternalScopeProvider _scopeProvider = new();
    public XUnitLoggerProvider(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    public ILogger CreateLogger(string? categoryName) => new XUnitLogger(this, _testOutputHelper, _scopeProvider, categoryName);
    public void Dispose() { }
    public int LogHistoryCapacity { get; set; } = 10000;
    private readonly ConcurrentQueue<LogHistoryEntry> _logHistory = new();

    public void AddToHistory(LogHistoryEntry logEntry)
    {
        while (_logHistory.Count >= LogHistoryCapacity) _logHistory.TryDequeue(out _);
        _logHistory.Enqueue(logEntry);
    }
    public IEnumerable<LogHistoryEntry> LogEntries => _logHistory;
}