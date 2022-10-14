// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System.Text;
using Microsoft.Extensions.Logging;
namespace StigsUtils.LoggingUtils;

public class MemoryLogger : ILogger {
	private readonly string? _categoryName;
	private readonly MemoryLog _memoryLog;
	private readonly LoggerExternalScopeProvider _scopeProvider;
	public MemoryLogger(LoggerExternalScopeProvider scopeProvider, string? categoryName, MemoryLog memoryLog) {
		_scopeProvider = scopeProvider;
		_categoryName = categoryName;
		_memoryLog = memoryLog;
	}
	public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) {
		var logEntry = new MemoryLogEntry {
			LogLevel = logLevel,
			EventId = eventId,
			State = state,
			Exception = exception,
		};
		var sb = new StringBuilder();
		sb.Append(GetLogLevelString(logLevel))
			.Append(" [").Append(_categoryName).Append("] ")
			.Append(formatter(state, exception));
		if (exception != null) sb.Append('\n').Append(exception);
		// Append scopes
		_scopeProvider.ForEachScope((scope, state) => {
			state.Append("\n => ");
			state.Append(scope);
		}, sb);
		logEntry.Message = sb.ToString();
		_memoryLog.Add(logEntry);
	}
	public bool IsEnabled(LogLevel logLevel) {
		return logLevel != LogLevel.None;
	}

	public IDisposable BeginScope<TState>(TState state) => _scopeProvider.Push(state);
		
	private static string GetLogLevelString(LogLevel logLevel) {
		return logLevel switch {
			LogLevel.Trace => "[T]",
			LogLevel.Debug => "[D]",
			LogLevel.Information => "[I]",
			LogLevel.Warning => "[W]",
			LogLevel.Error => "[E]",
			LogLevel.Critical => "[C]",
			_ => throw new ArgumentOutOfRangeException(nameof(logLevel))
		};
	}
}