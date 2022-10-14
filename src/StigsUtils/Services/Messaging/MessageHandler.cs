// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using Microsoft.Extensions.Logging;
namespace StigsUtils.Services.Messaging;

public interface IMessageHandler{ 
	public Task Execute(object? arg);
}

public interface IMessageHandler<in T> : IMessageHandler {
		public Task Execute(T? arg); 
}

public abstract class MessageMessageHandler<T> : IMessageHandler<T> {
	protected readonly ILogger _logger;
	protected MessageMessageHandler(ILogger logger) {
		_logger = logger;
	}

	public Task Execute(object? arg) { 
		if (arg != null) { 
			if (arg is T x) return Execute(x);
			_logger.LogError("Cannot receive message of type {MessageType}.",arg.GetType());
		} else _logger.LogError("Argument is null.");
		return Task.CompletedTask;
		
	}
	public Task Execute(T? arg) {
		if (arg != null) {
			_logger.LogTrace("Executing argument of type {Type}", arg.GetType());
			return OnExecute(arg);
		}
		_logger.LogError("Argument is null.");
		return Task.CompletedTask;
	}
	protected abstract Task OnExecute(T x);
}

