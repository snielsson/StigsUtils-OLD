// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.Logging;
namespace StigsUtils.DataTypes.Collections;

public class ActionQueue : IAsyncDisposable {
	public ActionQueue(ILogger log) {
		_log = log;
		_actionBlock = new ActionBlock<Func<Task>>(Execute, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 1 });
	}

	private readonly ActionBlock<Func<Task>> _actionBlock;
	private readonly ILogger _log;

	public async ValueTask DisposeAsync() {
		_actionBlock.Complete();
		await _actionBlock.Completion;
	}

	public bool Enqueue(Func<Task> queueItem) => _actionBlock.Post(queueItem);

	private async Task Execute(Func<Task> queueItem) {
		try {
			await queueItem();
		}
		catch (Exception ex) {
			_log.LogCritical("Unhandled exception while executing item on ActionQueue", ex);
		}
	}
}