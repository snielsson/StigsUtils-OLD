// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using StigsUtils.Extensions;
using Web.Api.Middleware.Commands;
namespace Web.Api.Middleware;

public static class Module {
	public static IServiceCollection AddWebApiMiddlewareModule(this IServiceCollection @this) => @this
		.Singleton<IGetAssemblyVersionQuery, GetAssemblyVersionQuery>();
}