// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.
namespace StigsUtils.Extensions;

public static class DelegateExtensions {
	public static Func<Task> ToAsyncFunc<T, U>(this Action<T, U> @this, T x, U y) => () => {
		@this(x, y);
		return Task.CompletedTask;
	};

	public static Func<Task> ToAsyncFunc<T>(this Action<T> @this, T x) => () => {
		@this(x);
		return Task.CompletedTask;
	};

	public static Func<Task> ToAsyncFunc(this Action @this) => () => {
		@this();
		return Task.CompletedTask;
	};
}