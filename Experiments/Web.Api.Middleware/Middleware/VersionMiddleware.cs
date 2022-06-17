// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using Web.Api.Middleware.Commands;
namespace Web.Api.Middleware.Middleware; 

public static class VersionMiddleware {

	// For .Net Core 3.1 etc based on IApplicationBuilder 
	public static IApplicationBuilder AddVersionEndpoint(this IApplicationBuilder @this) {
		@this.Map("/version", x => x.Run(WriteResponse));
		return @this;
	}

	
	// For modern .Net6 based on WebApplicationBuilder and WebApplication.
	public static WebApplication AddVersionEndpoint(this WebApplication @this) {
		@this.Map("/version",WriteResponse);
		return @this;
	}

	private static async Task WriteResponse(HttpContext httpContext) {
		var query = httpContext.RequestServices.GetRequiredService<IGetAssemblyVersionQuery>();
		await httpContext.Response.WriteAsJsonAsync(new { Version = query.Execute() });
	}
}