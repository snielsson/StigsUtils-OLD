// Copyright © 2021 Stig Schmidt Nielsson. This file is open source and distributed under the MIT License, see LICENSE file.

using System.Text;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
namespace StigsUtils.AspNet.Middleware.LogViewer {

	public class LogViewerMiddleware {
		public delegate IEnumerable<FileInfo> LogFileInfoGetter();
		private readonly LogFileInfoGetter _logFileInfoGetter;
		private readonly RequestDelegate _next;
		public LogViewerMiddleware(IConfiguration configuration, LogFileInfoGetter logFileInfoGetter, RequestDelegate next, string requestPath) {
			Configuration = configuration;
			RequestPath = requestPath;
			_logFileInfoGetter = logFileInfoGetter;
			_next = next;
		}
		private IConfiguration Configuration { get; }
		private string RequestPath { get; }
		public Task InvokeAsync(HttpContext context) {
			if (context.Request.Path.Value.ToLower() == RequestPath) {
				var files = context.Request.Query["file"].ToArray();
				if (files.Length == 0) return LogFileListHtmlPage(context.Response, _logFileInfoGetter());
				return LogFileContentHtmlPage(context.Response, files);
			}
			return _next(context);
		}

		private Task LogFileContentHtmlPage(HttpResponse r, string[] filePaths) {
			StringBuilder sb = new($@"<html><body><div><a href=""{RequestPath}"">Back to log file list.</a></div>");
			foreach (var filePath in filePaths) {
				sb.AppendLine($@"<h1>Contents of {Path.GetFileName(filePath)}</h1>");
				sb.AppendLine("<pre>");
				using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
					using (StreamReader reader = new(stream)) sb.AppendLine(reader.ReadToEnd());
				}
				sb.AppendLine("</pre>");
			}
			sb.AppendLine("</body></html>");
			r.StatusCode = 200;
			r.ContentType = "text/html;charset=utf-8";
			return r.WriteAsync(sb.ToString(), Encoding.UTF8);
		}

		private Task LogFileListHtmlPage(HttpResponse r, IEnumerable<FileInfo> fileInfos) {
			StringBuilder sb = new(@"<html><body><h1>Log Files</h1>");
			sb.AppendLine();
			foreach (FileInfo fileInfo in fileInfos) {
				if (!File.Exists(fileInfo.FullName)) throw new InvalidOperationException($"Log file {fileInfo.FullName} does not exist.");
				sb.AppendLine($@"<div><a href=""{RequestPath}?file={HttpUtility.UrlEncode(fileInfo.FullName)}"">{Path.GetFileName(fileInfo.FullName)}</a></div>");
			}
			sb.AppendLine("</body></html>");
			r.StatusCode = 200;
			r.ContentType = "text/html;charset=utf-8";
			return r.WriteAsync(sb.ToString(), Encoding.UTF8);
		}
	}
}