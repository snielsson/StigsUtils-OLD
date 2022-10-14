// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console.Cli;
using StigsUtils.Extensions;

namespace StigsUtils.SpectreConsole; 

public interface IConsoleApp  {
	Task<int> Run(string args = "") => Run(args.WhiteSpaceSplit());
	Task<int> Run(IEnumerable<string> args);
	IConsoleApp Configure(Action<IConfigurator> configurator);

	IConsoleApp ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate);

	IConsoleApp ConfigureServices(Action<IServiceCollection> configureDelegate);

	IConsoleApp SetApplicationName(string name);

	IConsoleApp AddCommand<T>(string? name = null, string? description = null, string? exampleArgs = null, bool isHidden = false) where T : class, ICommand;

	IConsoleApp SetFigletText(string text);
}