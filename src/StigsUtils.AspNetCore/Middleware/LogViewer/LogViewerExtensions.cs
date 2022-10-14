// Copyright © 2021 Stig Schmidt Nielsson. This file is open source and distributed under the MIT License, see LICENSE file.

using Microsoft.AspNetCore.Builder;
namespace StigsUtils.AspNet.Middleware.LogViewer {
	public static class LogViewerExtensions {
		public static IApplicationBuilder UseLogViewer(this IApplicationBuilder @this, string path = "/logs") {
			@this.UseMiddleware<LogViewerMiddleware>(path);
			return @this;
		}
	}
}