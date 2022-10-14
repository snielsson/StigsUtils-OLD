// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System.ComponentModel;
using Spectre.Console.Cli;
namespace StigsUtils.TestRunner.Commands; 

public class SomeOtherCommand : AsyncCommand<SomeOtherCommand.Settings> {
	private readonly ITimeService _timeService;
	public SomeOtherCommand(ITimeService timeService) {
		_timeService = timeService;

	}
	
	public class Settings : CommandSettings {
			[Description("some description about the name argument")]
			[CommandArgument(0, "[name]")]
			public string? Name { get; set; }

			[CommandOption("-c|--count")]
			[Description("some description about the count option")]
			[DefaultValue(1)]
												
			public int? Count { get; set; }
	}

	public override Task<int> ExecuteAsync(CommandContext context, Settings settings) {
		
		XUnitService.Run();
		
		return Task.FromResult(0);
	}
}