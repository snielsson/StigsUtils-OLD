// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StigsUtils.DataTypes.Exceptions;
namespace StigsUtils.Extensions;

public static class ServiceCollectionExtensions {

	/// <summary>
	/// Safe and fluent registration of a singleton dependency.
	/// If TService is not already registered, it is registered as a singleton using TImplementation as the ImplementationType.
	/// If already registered with the exact same types, nothing is done. 
	/// If registered with other types, an Error{TService} is thrown. 
	/// </summary>                                       
	/// <returns>The extended IServiceCollection for fluent usage.</returns>
	public static IServiceCollection Singleton<TService, TImplementation>(this IServiceCollection @this) {
		var serviceType = typeof(TService);
		var existingRegistrations = @this.Where(x => x.ServiceType == serviceType).ToArray();
		
		if (existingRegistrations.Length > 1) {
			var msg = $"Multiple registrations found for {serviceType} when attempting to register Singleton<{serviceType.Name},{typeof(TImplementation).Name}>.";
			throw new Error<IServiceCollection>(@this, msg);
		}

		if (existingRegistrations.Length == 1 && typeof(TImplementation) != existingRegistrations[0].ImplementationType) {
			var msg = $"Conflicting registration when attempting to register Singleton<{serviceType.Name},{typeof(TImplementation).Name}>: {existingRegistrations[0]}";
			throw new Error<IServiceCollection>(@this, msg);
		}
		
		@this.TryAddSingleton(typeof(TService), typeof(TImplementation));
		return @this;
	}

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