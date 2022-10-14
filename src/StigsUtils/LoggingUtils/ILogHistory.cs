// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.

using Microsoft.Extensions.Logging;
namespace StigsUtils.LoggingUtils;

public interface ILogHistory
{
    IEnumerable<LogHistoryEntry> LogEntries { get; }
}

public class MemoryLogEntry {
		public LogLevel LogLevel { get; init; }
		public EventId EventId { get; init; }
		public object? State { get; init; }
		public Exception? Exception { get; init; }
		public string? Message { get; internal set; }
	}

	public class MemoryLog : ILoggerProvider {
		private readonly List<MemoryLogEntry> _logEntries = new();
		private readonly LoggerExternalScopeProvider _scopeProvider = new();
		public IReadOnlyList<MemoryLogEntry> LogEntries => _logEntries;
		public void Dispose() { }
		public ILogger CreateLogger(string categoryName) => new MemoryLogger(_scopeProvider, categoryName, this);
		public void Add(MemoryLogEntry memoryLogEntry) {
			lock (_logEntries) {
				_logEntries.Add(memoryLogEntry);
				OnLogEntryAddedEvent(memoryLogEntry);
			}
			
		}
		
		public event Action<MemoryLogEntry, IReadOnlyList<MemoryLogEntry>>? LogEntryAddedEvent;
		protected void OnLogEntryAddedEvent(MemoryLogEntry memoryLogEntry) {
			Action<MemoryLogEntry, IReadOnlyList<MemoryLogEntry>>? @event = LogEntryAddedEvent;
			@event?.Invoke(memoryLogEntry, LogEntries);
		}
	}

	public class MemoryLogger<T> : MemoryLogger, ILogger<T> {
		public MemoryLogger(LoggerExternalScopeProvider scopeProvider, MemoryLog memoryLog) : base(scopeProvider, typeof(T).FullName, memoryLog) { }
	}

	// public static class MemoryLoggerExtensions {
	// 	public static IHostBuilder AddMemoryLogger(this IHostBuilder @this) {
	// 		@this.ConfigureLogging((context, loggingBuilder) => loggingBuilder.AddConfiguration(context.Configuration));
	// 		var memoryLog = new MemoryLog();
	// 		return @this.ConfigureServices(s => s
	// 			.AddSingleton(memoryLog)
	// 			.AddSingleton<ILoggerProvider>(_ => memoryLog));
	// 	}
	// 	public static IServiceCollection AddMemoryLogger(this IServiceCollection @this) {
	// 		var memoryLog = new MemoryLog();
	// 		return @this
	// 			.AddSingleton(memoryLog)
	// 			.AddSingleton<ILoggerProvider>(_ => memoryLog);
	// 	}
	// }