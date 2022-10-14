// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.Logging;
using StigsUtils.TimeUtils;
namespace StigsUtils.Services.Messaging;

public interface IMessageQueue { 
	int QueueCount {get;}
	int ExecutingCount { get; }
	int MaxCapacity {get;}
	int CurrentCapacity => MaxCapacity - QueueCount;
	bool HasCapacity => CurrentCapacity > 0;
}

public interface IMessageQueue<in T> : IMessageQueue  where T:IMessage<Guid,T>  {
	public Task<bool> AddAsync(T message);
}

internal abstract class MessageQueueBase<T> : IMessageQueue<T> where T:IMessage<Guid,T> {
	private readonly ILogger _logger;
	public int QueueCount { get; }
	private int _executingCount;
	public int ExecutingCount => _executingCount;
	public int MaxCapacity => _options.BoundedCapacity;
	public int MaxDegreeOfParallelism => _options.MaxDegreeOfParallelism;
	private ActionBlock<T> _actionBlock;
	private readonly ExecutionDataflowBlockOptions _options;

	protected MessageQueueBase(ILogger logger, ExecutionDataflowBlockOptions? options = null) {
		_logger = logger;
		_options = options ?? new ExecutionDataflowBlockOptions();
		_actionBlock = new ActionBlock<T>(Execute, _options);
	}

	public async Task<bool> AddAsync(T message) {
		_logger.Log(LogLevel, "Adding message {MessageId} on message queue.", message.Id);
		return await _actionBlock.SendAsync(message);
	}

	public LogLevel LogLevel { get; set; } = LogLevel.Trace;
	
	public override string ToString() => $"MessageQueue<{typeof(T).Name}>. Queued: {QueueCount}. Executing;{ExecutingCount}.";

	async Task Execute(T message) {
		var time = new TimeMeasure();
		Interlocked.Increment(ref _executingCount);
		try {
			_logger.Log(LogLevel, "Executing message {MessageId} on {MessageQueue}", message.Id, this);
			await OnExecute(message);
			_logger.Log(LogLevel, "Executed message {MessageId} on {MessageQueue}. Elapsed={Elapsed}ms.", message.Id, this, time.ElapsedMs);
		}
		catch(Exception ex) {
			_logger.LogCritical(ex, "Unhandled Exception executing message {MessageId} on {MessageQueue}. Elapsed={Elapsed}ms.", message.Id, this, time.ElapsedMs);
		}
		finally{
			Interlocked.Decrement(ref _executingCount);
		}
	}
	protected abstract Task OnExecute(T message);
}

