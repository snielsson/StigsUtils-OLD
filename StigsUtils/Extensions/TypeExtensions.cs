// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.
namespace StigsUtils.Extensions;

public static class TypeExtensions {
	//IsOfGenericType created from copy of https://stackoverflow.com/a/29823390/193414.
	public static bool IsOfGenericType(this Type @this, Type t) => @this.IsOfGenericType(t, out _);

	public static bool IsOfGenericType(this Type @this, Type t, out Type? concreteGenericType) {
		while (true) {
			concreteGenericType = null;
			if (t == null) throw new ArgumentNullException(nameof(t));
			if (!t.IsGenericTypeDefinition) throw new ArgumentException($"The type {t} is not generic", nameof(t));
			if (@this == null || @this == typeof(object)) return false;
			if (@this == t) {
				concreteGenericType = @this;
				return true;
			}
			if ((@this.IsGenericType ? @this.GetGenericTypeDefinition() : @this) == t) {
				concreteGenericType = @this;
				return true;
			}
			if (t.IsInterface) {
				foreach (Type i in @this.GetInterfaces())
					if (i.IsOfGenericType(t, out concreteGenericType))
						return true;
			}
			if (@this.BaseType == null) return false;
			@this = @this.BaseType;
		}
	}
}