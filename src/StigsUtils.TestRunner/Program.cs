// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using Microsoft.Extensions.DependencyInjection;
using StigsUtils.TestRunner.Commands;
var exitCode = await ConsoleApp.ConfigureServices(services => services
		.AddSingleton<ITimeService, TimeService>()
	)
	.AddCommand<SomeOtherCommand>("xyz", "some other description", "a -c 45")
	.SetFigletText("TestRunner")
	.SetApplicationName("testrunner.exe")
	.Run("xyz");
#if DEBUG
Console.WriteLine("Press key to exit...");
Console.ReadKey();
#endif
return exitCode;
