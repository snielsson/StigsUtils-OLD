// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using FluentAssertions;
using FluentAssertions.Collections;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.Core;
namespace StigsUtils.TestUtils.FluentAssertions;

public static class FluentAssertionExtensions {
	public static AndConstraint<GenericCollectionAssertions<ICall>> ShouldContain(this ILogger @this, LogLevel logLevel, string messageStart) =>
		@this.ReceivedCalls()
			.Where(x => x.GetMethodInfo().Name == "Log")
			.Where(x => (LogLevel) x.GetArguments()[0]! == logLevel)
			.Where(x => x.GetArguments()[2]?.ToString()?.StartsWith(messageStart) ?? false)
			.Should()
			.NotBeEmpty($"Logger has not received the expected log message with level {logLevel} and message starting with '{messageStart}'.");
}