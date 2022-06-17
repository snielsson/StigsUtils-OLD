﻿// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
namespace StigsUtils.TestUtils.XUnit;

public abstract class XUnitTestBase : IDisposable {
	protected XUnitTestBase(ITestOutputHelper output, IServiceCollection? services = null) {
		Output = output;
		Services = services ?? new ServiceCollection();
		_serviceProvider = new Lazy<IServiceProvider>(() => Services.BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true }));
		Output.WriteLine("Test start time: " + DateTime.Now);
	}

	public void Dispose() {
		Output.WriteLine("Test end time: " + DateTime.Now);
	}

	private readonly Lazy<IServiceProvider> _serviceProvider;
	protected ITestOutputHelper Output { get; }
	public IServiceCollection Services { get; }
	public IServiceProvider ServiceProvider => _serviceProvider.Value;
}