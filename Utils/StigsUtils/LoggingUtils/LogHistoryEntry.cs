// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.

using Microsoft.Extensions.Logging;

namespace StigsUtils.LoggingUtils;

public class LogHistoryEntry
{
    public LogLevel LogLevel { get; set; }
    public EventId EventId { get; set; }
    public string? Message { get; set; }
    public Exception? Exception { get; set; }
}