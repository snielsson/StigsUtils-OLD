// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
namespace StigsUtils.Extensions;

[Obsolete("not tested")]
public static class ReaderWriterLockSlimExtensions {
	public static void WriteAccess(this ReaderWriterLockSlim @this, Action action, int timeOut = 5000) {
		if (!@this.TryEnterWriteLock(timeOut)) throw new TimeoutException($"Could not get write access in {timeOut}ms.");
		try {
			action.Invoke();
		}
		finally {
			@this.ExitWriteLock();
		}
	}

	public static T ReadAccess<T>(this ReaderWriterLockSlim @this, Func<T> func, int timeOut = 5000) {
		if (!@this.TryEnterReadLock(timeOut)) throw new TimeoutException($"Could not get write access in {timeOut}ms.");
		try {
			@this.EnterReadLock();
			return func.Invoke();
		}
		finally {
			@this.ExitReadLock();
		}
	}
}