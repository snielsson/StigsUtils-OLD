// Copyright © 2021 Stig Schmidt Nielsson. This file is open source and distributed under the MIT License, see LICENSE file.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using Serilog.Events;
using Serilog.Formatting.Compact.Reader;

namespace StigsUtils.Serilog {
	public static class SerilogFunctions {
		public static IEnumerable<string> GetLogFilePaths(IConfiguration configuration, string? fileNamePattern = null, string fileSinkName = "File") {
			IConfigurationSection? fileSinkConfiguration = configuration.GetSection("Serilog:WriteTo").GetChildren().SingleOrDefault(x => x["Name"] == fileSinkName);
			if (fileSinkConfiguration == null) throw new InvalidOperationException("Could not find configuration for File log, so cannot get log file contents.");
			var pathConfig = fileSinkConfiguration["Args:path"];
			var dir = Path.GetDirectoryName(pathConfig);
			if (dir == null) throw new Exception($"Could not determine log directory from configuration, pathConfig = {pathConfig}.");
			if(!Directory.Exists(dir)) return Array.Empty<string>();
			fileNamePattern ??= Path.GetFileNameWithoutExtension(pathConfig) + "*" + Path.GetExtension(pathConfig);
			return Directory.EnumerateFiles(dir, fileNamePattern).OrderBy(x => x); // Consider to use real regex instead of simple file pattern.
		}

		public static byte[] RenderLogFile(string filePath) {
			using var streamReader = File.OpenText(filePath);
			var fileContents = RenderCompressedJsonSeriLogFile(streamReader)
				.SelectMany(x => Encoding.UTF8.GetBytes(x))
				.ToArray();
			return fileContents;
		}

		public static string RenderCompressedJsonSeriLogFile(string filepath) {
			using var streamReader = File.OpenText(filepath);
			return string.Join("\n", RenderCompressedJsonSeriLogFile(streamReader));
		}

		public static IEnumerable<string> RenderCompressedJsonSeriLogFile(StreamReader streamReader) {
			using var reader = new LogEventReader(streamReader);
			while (reader.TryRead(out LogEvent? logEvent)) yield return $"{logEvent.Timestamp}:{logEvent.Level}:{logEvent.RenderMessage()}\n";
		}
	}
}