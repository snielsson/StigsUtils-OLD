// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
namespace StigsUtils.Extensions;

public static class ObjectExtensions {

	public static T Assert<T>(this T @this, bool predicate, Func<T, string>? errorMsg = null) {
		if (!predicate) throw new Exception(errorMsg?.Invoke(@this) ?? $"Assert failed on {@this}.");
		return @this;
	}
	//TEST
	public static T Assert<T>(this T @this, Func<T, bool> predicate, Func<T, string>? errorMsg = null) {
		if (!predicate(@this)) throw new Exception(errorMsg?.Invoke(@this) ?? $"Assert failed on {@this}.");
		return @this;
	}	
	public static T NotNull<T>(this T @this, string? msg = null) {
		if (@this != null) throw new Exception(msg ?? $"{@this} of type {typeof(T)} is not null.");
		return @this;
	}

	public static T Lock<T>(this T @this, Action action) {
		lock (@this!) {
			action();
			return @this;
		}
	}
	public static T Lock<T>(this T @this, object lockObject, Action action) {
		lock (lockObject.NotNull()) {
			action();
			return @this;
		}
	}

	public static T? JsonClone<T>(this T? @this) => @this!.ToJson().FromJson<T>() ;	
	
	public static Action<string> DumpAction { get; set; } = s => Console.Out.WriteLine(s);

	public static T Dump<T>([DisallowNull] this T @this, string? header = null, [CallerMemberName] string? caller = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = default) {
		if (@this == null) throw new ArgumentNullException(nameof(@this));
		DumpAction("======================================================================");
		DumpAction($"Dump called by {caller}.");
		DumpAction($"Dump called at {Path.GetFileName(callerFilePath)} line {callerLineNumber}.");
		DumpAction("----------------------------------------------------------------------");
		if (header != null) DumpAction(header);
		DumpAction(@this.ToPrettyJson());
		return @this;
	}

	public static T SetProperty<T, TValue>(this T @this, string name, object value, bool caseSensitive = true, bool throwOnError = true)
		=> @this!.SetProperty(name, typeof(TValue), value, caseSensitive, throwOnError);

	public static T SetProperty<T>(this T @this, string name, object value, bool caseSensitive = true, bool throwOnError = true)
		=> @this!.SetProperty(name, value.GetType(), value, caseSensitive, throwOnError);

	public static T SetProperty<T>([DisallowNull] this T @this, string name, Type type, object value, bool caseSensitive = true, bool throwOnError = true) {
		if (@this == null) throw new ArgumentNullException(nameof(@this));
		StringComparison comparison = caseSensitive
			? StringComparison.InvariantCulture
			: StringComparison.InvariantCultureIgnoreCase;
		PropertyInfo? property = typeof(T).GetProperties().SingleOrDefault(x => x.Name.Equals(name, comparison));
		if (property != null) {
			property.SetMethod!.Invoke(@this, new[] { Convert.ChangeType(value, type) });
			return @this;
		}
		if (throwOnError) throw new Exception($"Setter for property '{name}' not found on {@this}.");
		return @this;
	}
}