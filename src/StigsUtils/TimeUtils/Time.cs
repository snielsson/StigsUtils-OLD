// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.
namespace StigsUtils.TimeUtils;

public interface ITime {
	UtcDateTime UtcNow { get; }
	// ITimeService CallAt(UtcDateTime val, Action action);
	// ITimeService CallAt(UtcDateTime val, Action<UtcDateTime> action);
	// ITimeService CallAt(UtcDateTime val, Action<UtcDateTime,UtcDateTime> action);
	// ITimeService CallIn(TimeSpan val, Action action);
	// ITimeService CallIn(TimeSpan val, Action<UtcDateTime> action);
	// ITimeService CallIn(TimeSpan val, Action<UtcDateTime, UtcDateTime> action);
}

public class Time : ITime {
	private readonly UtcDateTime? _frozenTime = null;
	public UtcDateTime UtcNow => _frozenTime ?? DateTime.UtcNow;

	// private readonly IQueueService _queueService;
	// public TimeService(IQueueService queueService) {
	// 	_queueService = queueService;
	// 	_timer = new Timer(TimerCallback);
	// }
	// private UtcDateTime? _frozenTimeStamp = null;
	// public bool IsTimeFrozen => _frozenTimeStamp != null;
	// public UtcDateTime FreezeTimeAt(UtcDateTime val, bool executeScheduledActions = true) {
	// 	_timer.Stop();
	// 	if (executeScheduledActions) { 
	// 		//TODO...
	// 	}
	// 	return (_frozenTimeStamp = val).Value;
	// }
	// public UtcDateTime UnFreezeTime(bool executeScheduledActions = true) {
	// 	_frozenTimeStamp = null;
	// 	TimerCallback();
	// 	return UtcNow;
	// }
	// private Timer _timer; 
	// private void TimerCallback(object? state=null) {
	// 	_timer.Stop();
	// 	var timeout = ExecuteScheduledActions(UtcNow);
	// 	if(!IsTimeFrozen) _timer.CallAfter(timeout);
	// }
	//
	// private TimeSpan ExecuteScheduledActions(UtcDateTime now, params object[] items) => ExecuteScheduledActions(now, (IEnumerable<object>) items);
	// private TimeSpan ExecuteScheduledActions(UtcDateTime now, IEnumerable<object> items) {
	// 	List<KeyValuePair<UtcDateTime,HashSet<object>>> itemsToExecute = new ();
	// 	lock (_scheduledActions) {
	// 		foreach (KeyValuePair<UtcDateTime, HashSet<object>> item in _scheduledActions) {
	// 			if (item.Key > now) break;
	// 			itemsToExecute.Add(item);
	// 		}
	// 		foreach (var item in itemsToExecute) _scheduledActions.Remove(item.Key);
	// 	}
	// 	
	// 	foreach (var (scheduledTime, actions) in itemsToExecute) {
	// 		foreach (object action in actions) {
	// 			switch (action) {
	// 				case Action a:
	// 					_queueService.Enqueue(a.ToAsyncFunc());
	// 					break;
	// 				case Action<UtcDateTime> a:
	// 					_queueService.Enqueue(a.ToAsyncFunc(now));
	// 					break;
	// 				case Action<UtcDateTime, UtcDateTime> a:
	// 					_queueService.Enqueue(a.ToAsyncFunc(now, scheduledTime));
	// 					break;
	// 			}
	// 		}
	// 	}
	// 	lock (_scheduledActions) {
	// 		if (_scheduledActions.IsEmpty()) return Timeout.InfiniteTimeSpan;
	// 		if (_scheduledActions.First().Key <= now) return TimeSpan.Zero;
	// 		return _scheduledActions.First().Key - now;
	// 	}
	// }
	//
	//
	// private ITimeService ScheduleAction(UtcDateTime newScheduleTime, object obj) {
	// 	var now = UtcNow;
	// 	//TODO: finish time service: when time resumed execute all scheduled actions as needed-
	// 	// if(newScheduleTime < now) return ExecuteScheduledActions(now, newScheduleTime, obj);
	// 	bool mustResetTimer;
	// 	lock (_scheduledActions) {
	// 		mustResetTimer = _scheduledActions.IsEmpty() || newScheduleTime < (_scheduledActions?.First().Key ?? DateTime.MaxValue);
	// 		if (!_scheduledActions.TryGetValue(newScheduleTime, out var items)) items = new HashSet<object>();
	// 		items.Add(obj);
	// 	}
	// 	if (mustResetTimer) _timer.Change(newScheduleTime-UtcNow,Timeout.InfiniteTimeSpan);
	// 	return this;
	// }
	// public UtcDateTime UtcNow => _frozenTimeStamp ?? DateTime.UtcNow;
	// public ITimeService CallAt(UtcDateTime val, Action action)
	// 	=> throw new NotImplementedException();
	// public ITimeService CallAt(UtcDateTime val, Action<UtcDateTime> action)
	// 	=> throw new NotImplementedException();
	// public ITimeService CallAt(UtcDateTime val, Action<UtcDateTime, UtcDateTime> action)
	// 	=> throw new NotImplementedException();
	// public ITimeService CallIn(TimeSpan val, Action action)
	// 	=> throw new NotImplementedException();
	// public ITimeService CallIn(TimeSpan val, Action<UtcDateTime> action)
	// 	=> throw new NotImplementedException();
	// public ITimeService CallIn(TimeSpan val, Action<UtcDateTime, UtcDateTime> action)
	// 	=> throw new NotImplementedException();
	// public SortedDictionary<UtcDateTime, HashSet<object>> _scheduledActions = new SortedDictionary<UtcDateTime, HashSet<object>>();
	//
	// private int _isDisposed;
	// public void Dispose() {
	// 	if(Interlocked.CompareExchange(ref _isDisposed, 1, 0) == 1) return;
	// 	_timer.Dispose();
	// }
}