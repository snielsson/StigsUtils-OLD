// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
public interface ITimeService {
	public DateTime UtcNow { get; }
};

public class TimeService : ITimeService { 
	public DateTime UtcNow => DateTime.UtcNow;
}