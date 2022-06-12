// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
namespace StigsUtils.Extensions;

public static class ServiceCollectionExtensions {
	public static IServiceCollection AddSingleton<TService1, TService2, TImplementation>(this IServiceCollection @this) where TImplementation : notnull, TService1, TService2 => @this
		.AddSingleton(typeof(TImplementation))
		.AddSingleton(typeof(TService1), x => x.GetRequiredService<TImplementation>())
		.AddSingleton(typeof(TService2), x => x.GetRequiredService<TImplementation>());

	public static IServiceCollection AddSingleton<TService1, TService2, TImplementation>(this IServiceCollection @this, Func<IServiceProvider, TImplementation> factoryMethod) where TImplementation : class, TService1, TService2 => @this
		.AddSingleton(factoryMethod)
		.AddSingleton(typeof(TService1), x => x.GetRequiredService<TImplementation>())
		.AddSingleton(typeof(TService2), x => x.GetRequiredService<TImplementation>());

	public static IServiceCollection AddSingleton<TService1, TService2, TService3, TImplementation>(this IServiceCollection @this) where TImplementation : notnull, TService1, TService2, TService3 => @this
		.AddSingleton(typeof(TImplementation))
		.AddSingleton(typeof(TService1), x => x.GetRequiredService<TImplementation>())
		.AddSingleton(typeof(TService2), x => x.GetRequiredService<TImplementation>())
		.AddSingleton(typeof(TService3), x => x.GetRequiredService<TImplementation>());

	public static IServiceCollection AddInterfaceImplementationsAsSingletons<T>(this IServiceCollection @this, Assembly assembly) =>
		AddInterfaceImplementationsAsSingletons(@this, typeof(T), assembly);

	public static IServiceCollection AddInterfaceImplementationsAsSingletons(this IServiceCollection @this, Type interfaceType, Assembly? assembly = null) {
		if (!interfaceType.IsInterface) throw new InvalidOperationException($"Generic type {interfaceType} is not an interface.");
		assembly ??= Assembly.GetCallingAssembly();
		if (interfaceType.IsGenericType) {
			Type interfaceTypeGenericTypeDefinition = interfaceType.GetGenericTypeDefinition();
			IEnumerable<Type> implementingTypes = assembly.GetLoadableTypes().Where(type =>
				!type.IsAbstract &&
				!type.IsInterface &&
				type.GetInterfaces().Any(implementedInterface =>
					implementedInterface.IsGenericType &&
					implementedInterface.GetGenericTypeDefinition() == interfaceTypeGenericTypeDefinition));
			foreach (Type implementingType in implementingTypes) {
				Type serviceType = implementingType.GetInterfaces().Single(x => x.IsGenericType && x.GetGenericTypeDefinition() == interfaceTypeGenericTypeDefinition);
				@this.AddSingleton(serviceType, implementingType);
			}
		}
		return @this;
	}

	public static IServiceCollection AddGenericImplementations(this IServiceCollection @this, Type type, params Assembly[] assemblies) {
		foreach (Assembly assembly in assemblies) {
			IEnumerable<Type> implementations = assembly.GetLoadableTypes().Where(x =>
				!x.IsAbstract &&
				!x.IsInterface &&
				x.BaseType != null &&
				x.BaseType.IsGenericType &&
				x.BaseType.GetGenericTypeDefinition() == type);
			foreach (Type implementation in implementations) {
				Type serviceType = type.MakeGenericType(implementation.BaseType!.GetGenericArguments());
				@this.AddSingleton(serviceType, implementation);
			}
		}
		return @this;
	}
}