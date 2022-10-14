// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
namespace StigsUtils.Extensions;

public static class ObjectExtensions {

	public static Action<string> DumpAction { get; set; } = s => Console.Out.WriteLine(s);

	public static T Assert<T>(this T @this, Func<T, bool> assertion, string errorMessage) => @this.Assert(assertion, _ => errorMessage);

	public static T Assert<T>(this T @this, bool predicate, Func<T, string>? errorMsg = null) {
		if (!predicate) throw new Exception(errorMsg?.Invoke(@this) ?? $"Assert failed on {@this}.");
		return @this;
	}

	public static T Assert<T>(this T @this, Func<T, bool> predicate, Func<T, string>? errorMsg = null) {
		if (!predicate(@this)) throw new Exception(errorMsg?.Invoke(@this) ?? $"Assert failed on {@this}.");
		return @this;
	}

	public static T AssertNotNull<T>(this T @this, string? name = null) {
		if (@this == null) throw new Exception($"{name ?? "Value"} of type {typeof(T)} cannot be null");
		return @this;
	}

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

	public static T EnsureValid<T>([DisallowNull] this T @this, string? message = null, ValidationContext? validationContext = null) {
		if (@this == null) throw new ArgumentNullException(nameof(@this));
		var validationResults = new List<ValidationResult>();
		validationContext ??= new ValidationContext(@this);
		Validator.TryValidateObject(@this, validationContext, validationResults);
		var validatableObject = @this as IValidatableObject;
		validatableObject?.Validate(validationContext).AddTo(validationResults);
		if (validationResults.Count == 0) return @this;
		throw new ValidationException(validationResults.ToPrettyJson());
	}

	public static int HashCombine<T, TU>([DisallowNull] this T @this, params TU[] objs) {
		if (@this == null) throw new ArgumentNullException(nameof(@this));
		var result = @this.GetHashCode();
		for (var i = 0; i < objs.Length; ++i) {
			TU obj = objs[i];
			if (obj != null) result = (result * 397) ^ obj.GetHashCode();
		}
		return result;
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

	public static T NotNull<T>(this T @this, string? msg = null) {
		if (@this != null) throw new Exception(msg ?? $"{@this} of type {typeof(T)} is not null.");
		return @this;
	}

	public static T SetProperty<T, TValue>(this T @this, string name, object value, bool caseSensitive = true, bool throwOnError = true) => @this!.SetProperty(name, typeof(TValue), value, caseSensitive, throwOnError);

	public static T SetProperty<T>(this T @this, string name, object value, bool caseSensitive = true, bool throwOnError = true) => @this!.SetProperty(name, value.GetType(), value, caseSensitive, throwOnError);

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

	public static HttpContent ToJsonContent(this object @this, bool pretty = true, Encoding? encoding = null) {
		if (@this == null) throw new ArgumentNullException(nameof(@this));
		return new StringContent(@this.ToPrettyJson(), encoding ?? Encoding.UTF8, MediaTypeNames.Application.Json);
	}
}