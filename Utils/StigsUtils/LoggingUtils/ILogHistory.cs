// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.

namespace StigsUtils.LoggingUtils;

public interface ILogHistory
{
    IEnumerable<LogHistoryEntry> LogEntries { get; }
}