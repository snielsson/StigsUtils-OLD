// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
namespace Web.Api.Middleware.Commands;

public interface IGetAssemblyVersionQuery {
	string Execute();
}

public class GetAssemblyVersionQuery : IGetAssemblyVersionQuery {
	public string Execute() => "some version string - " + DateTime.UtcNow;
}