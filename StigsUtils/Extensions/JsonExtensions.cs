// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.
using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace StigsUtils.Extensions;

public static class JsonExtensions {

	public static JsonSerializerSettings DefaultPrettyJsonSerializerSettings { get; set; } = new() {
		Formatting = Formatting.Indented
	};
	public static JsonSerializerSettings DefaultJsonSerializerSettings { get; set; } = new() {
		Formatting = Formatting.None
	};

	public static string ToPrettyJson(this object @this, JsonSerializerSettings? settings = null) => JsonConvert.SerializeObject(@this, settings ?? DefaultPrettyJsonSerializerSettings);

	public static string ToJson(this object @this) => JsonConvert.SerializeObject(@this, DefaultJsonSerializerSettings);
	
	public static string JsonFormat(this string @this, bool indented = true) => JToken.Parse(@this).ToString(indented ? Formatting.Indented : Formatting.None);
	
	public static HttpContent AsJsonHttpContent(this string @this, Encoding? encoding = null)
	{
		var result = new StringContent(@this, encoding ?? Encoding.UTF8, MediaTypeNames.Application.Json);
		return result;
	}
}