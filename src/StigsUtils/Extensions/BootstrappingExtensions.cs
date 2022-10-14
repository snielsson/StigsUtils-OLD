// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace StigsUtils.Extensions;

public static class BootstrappingExtensions {
	// //TODO: lav om til extension metode på assembly - se cloud lens.	
	// public static IServiceCollection AddAssemblyConfigurationFiles<T>(this IServiceCollection @this, string? environment = null, string configFileDir = "ConfigurationFiles", string? fileName = null)
	// 	=> @this.AddSingleton(typeof(T), x => x.LoadAssemblyConfigurationFiles<T>(environment, configFileDir, fileName) ?? throw new InvalidOperationException());
	//
	// public static IServiceCollection AddAssemblyConfigurationFiles<TService, TImplementation>(this IServiceCollection @this, string? environment = null, string configFileDir = "ConfigurationFiles", string? fileName = null)
	// 	=> @this.AddSingleton(typeof(TService), x => x.LoadAssemblyConfigurationFiles<TImplementation>(environment, configFileDir, fileName) ?? throw new InvalidOperationException());

	private static ServiceDescriptor GetServiceDescriptor<TService>(this IServiceCollection @this)
		=> @this.GetServiceDescriptors<TService>().Single();

	private static ServiceDescriptor? GetServiceDescriptorOrDefault<TService>(this IServiceCollection @this)
		=> @this.GetServiceDescriptors<TService>().SingleOrDefault();

	private static IEnumerable<ServiceDescriptor> GetServiceDescriptors<TService>(this IServiceCollection @this)
		=> @this.Where(x => x.ServiceType == typeof(TService));

	// public static T LoadAssemblyConfigurationFiles<T>(this IServiceProvider @this, string? environment = null, string configFileDir = "ConfigurationFiles", string? fileName = null) {
	// 	environment ??= @this.GetService<IHostEnvironment>()?.EnvironmentName ?? Environments.Development;
	// 	return LoadAssemblyConfigurationFiles<T>(environment, configFileDir, fileName);
	// }
	//
	// public static T LoadAssemblyConfigurationFiles<T>(string environmentName, string configFileDir = "ConfigurationFiles", string? fileName = null, bool throwOnNoFiles = true) {
	// 	var assembly = Assembly.GetAssembly(typeof(T));
	// 	var baseFilePath = Path.Combine(Path.GetDirectoryName(assembly.Location)!, configFileDir, (fileName ?? assembly.GetName().Name)!);
	// 	var files = new[] {
	// 		$"{baseFilePath}.json",
	// 		$"{baseFilePath}.Default.json",
	// 		$"{baseFilePath}.{environmentName}.json"
	// 	}.Where(File.Exists).ToArray();
	// 	if (throwOnNoFiles && files.Length == 0) throw new Exception($"No config files found for assembly {assembly.GetName().Name}, baseFilePath = {baseFilePath}");
	// 	IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
	// 	foreach (var file in files) configurationBuilder.AddJsonFile(file, false);
	// 	var result = configurationBuilder.Build().Get<T>();
	// 	return result;
	// }

	private static ServiceLifetime RemoveRegistration<T>(this IServiceCollection @this) {
		ServiceDescriptor? serviceDescriptor = @this.GetServiceDescriptorOrDefault<T>();
		@this.Remove(serviceDescriptor ?? throw new InvalidOperationException(""));
		return serviceDescriptor.Lifetime;
	}

	public static IServiceCollection Replace<TService, TImplementation>(this IServiceCollection @this) {
		ServiceLifetime lifetime = @this.RemoveRegistration<TService>();
		@this.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), lifetime));
		return @this;
	}

	public static IServiceCollection Replace<TService>(this IServiceCollection @this, Func<IServiceProvider, object> func) {
		ServiceLifetime lifetime = @this.RemoveRegistration<TService>();
		@this.Add(new ServiceDescriptor(typeof(TService), func, lifetime));
		return @this;
	}
}