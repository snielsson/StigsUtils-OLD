// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console;
using Spectre.Console.Cli;
using StigsUtils.Extensions;
using StigsUtils.SpectreConsole;

public sealed class ConsoleApp : IConsoleApp, ITypeRegistrar, ITypeResolver, ICommandInterceptor {
	private readonly IHostBuilder _hostBuilder;
	private readonly CommandApp _commandApp;
	private Lazy<IServiceProvider> _serviceProvider;
	private readonly Lazy<IHost> _host;
	private FigletText? _figletText;

	public ConsoleApp(IHostBuilder? hostBuilder = null) {
		_hostBuilder = hostBuilder ?? Host.CreateDefaultBuilder();
		_host = new Lazy<IHost>(() => _hostBuilder.Build());
		_serviceProvider = new Lazy<IServiceProvider>(() => _host.Value.Services);
		_commandApp = new CommandApp(this);
		_commandApp.Configure(configurator => {
			
			configurator.SetInterceptor(this);
#if DEBUG
			configurator.PropagateExceptions();
			configurator.ValidateExamples();
#endif
			configurator.SetExceptionHandler(ex => -1);
		});
	}

	public IConsoleApp ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate) {
		_hostBuilder.ConfigureServices(configureDelegate);
		return this;
	}

	public IConsoleApp ConfigureServices(Action<IServiceCollection> configureDelegate) {
		_hostBuilder.ConfigureServices(configureDelegate);
		return this;
	}
	
	
	
	public static IConsoleApp ConfigureServices(Action<IServiceCollection> configureDelegate, IConsoleApp? app = null) => (app ??new ConsoleApp()).ConfigureServices(configureDelegate);

	public IConsoleApp AddCommand<T>(string? name = null, string? description = null, string? exampleArgs = null, bool isHidden = false) where T : class, ICommand {
		if (exampleArgs != null && !exampleArgs.StartsWith($"{name} ")) exampleArgs = $"{name} {exampleArgs}";
	// 	return AddCommand<T>(name, description, exampleArgs?.WhiteSpaceSplit() ?? Array.Empty<string>(), isHidden);
	// }
	//
	// private IConsoleApp AddCommand<T>(string? name, string? description, IEnumerable<string>? exampleArgs, bool isHidden) where T : class, ICommand {
		name ??= typeof(T).Name.TrimEnd("Command").ToLower();
		_commandApp.Configure(configurator => {
			var command = configurator.AddCommand<T>(name);
			if (description != null) command.WithDescription(description);
			if(exampleArgs != null) command.WithExample(exampleArgs.WhiteSpaceSplit().ToArray());
			if (isHidden) command.IsHidden();
		});
		return this;
	}

	public IConsoleApp Configure(Action<IConfigurator> configurator) {
		_commandApp.Configure(configurator);
		return this;
	}

	public IConsoleApp SetApplicationName(string name) { 
		_commandApp.Configure(configurator => configurator.SetApplicationName(name));
		return this;
	}

	public IConsoleApp SetFigletText(string text) => SetFigletText(new FigletText(text));
	public IConsoleApp SetFigletText(FigletText figletText) {
		_figletText = figletText;
		return this;
	}

	public async Task<int> Run(IEnumerable<string> args) => await _commandApp.RunAsync(args);

	public IServiceProvider ServiceProvider => _serviceProvider.Value;

	public ITypeResolver Build() => this;

	public void Register(Type service, Type implementation) => _hostBuilder.ConfigureServices(x => x.AddSingleton(service, implementation));

	public void RegisterInstance(Type service, object implementation) => _hostBuilder.ConfigureServices(x => x.AddSingleton(service, implementation));

	public void RegisterLazy(Type service, Func<object> func) {
		if (func is null) throw new ArgumentNullException(nameof(func));
		_hostBuilder.ConfigureServices(x => x.AddSingleton(_ => func()));
	}

	public object? Resolve(Type? type) => ServiceProvider.GetService(type ?? throw new ArgumentNullException(nameof(type)));

	public void Intercept(CommandContext context, CommandSettings settings) {
		
		if(_figletText != null) AnsiConsole.Write(_figletText);
		
	}
}
