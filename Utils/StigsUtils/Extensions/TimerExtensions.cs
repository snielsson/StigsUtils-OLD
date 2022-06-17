// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.
namespace StigsUtils.Extensions;

public static class TimerExtensions {
	public static Timer Stop(this Timer @this) {
		@this.Change(Timeout.Infinite, Timeout.Infinite);
		return @this;
	}

	public static Timer CallAfter(this Timer @this, TimeSpan timeSpan) {
		@this.Change(timeSpan, Timeout.InfiniteTimeSpan);
		return @this;
	}
}