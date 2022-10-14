// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace StigsUtils.FileUtils;

public static class Resource {
	public static Stream? Open(string resourcePath, Assembly? assembly = null) { 
		assembly ??= Assembly.GetCallingAssembly();
		var assemblyName = assembly.GetName().Name!;
		if (!resourcePath.StartsWith(assemblyName)) resourcePath = $"{assemblyName}.{resourcePath.TrimStart('.')}";
		return assembly.GetManifestResourceStream(resourcePath);
	}
	
	public static string ReadAllText(string resourcePath, Assembly? assembly = null) { 
		using Stream s = Open(resourcePath, assembly ?? Assembly.GetCallingAssembly())!;
		using StreamReader sr = new(s);
		return sr.ReadToEnd();
	}
	
	public static JToken LoadJson(string resourcePath, Assembly? assembly = null) { 
		using Stream s = Open(resourcePath, assembly ?? Assembly.GetCallingAssembly())!;
		using StreamReader sr = new(s);
		using JsonReader j = new JsonTextReader(sr);
		return JToken.Load(j);
	}
}