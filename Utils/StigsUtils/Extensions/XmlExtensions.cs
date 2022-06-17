// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using StigsUtils.Xml;
namespace StigsUtils.Extensions;

public static class XmlExtensions {
	public static string ToXml<T>(this T @this, string rootTag, string defaultNamespace, XmlSerializerNamespaces namespaces) {
		using (var stream = new MemoryStream()) {
			var root = new XmlRootAttribute(rootTag) {
				Namespace = defaultNamespace
			};
			var xmlSerializer = CachingXmlSerializerFactory.Create(typeof(T), root);
			xmlSerializer.Serialize(stream, @this, namespaces);
			stream.Position = 0;
			using (var reader = new StreamReader(stream)) {
				var result = reader.ReadToEnd();
				return result;
			}
		}
	}

	/// <summary>
	///     Deserialize XML string, optionally only an inner fragment of the XML, as specified by the innerStartTag parameter.
	/// </summary>
	public static T? FromXml<T>(this XmlReader @this, string rootTag, string defaultNamespace = "") {
		var root = new XmlRootAttribute(rootTag) {
			Namespace = defaultNamespace
		};
		var xmlSerializer = CachingXmlSerializerFactory.Create(typeof(T), root);
		return (T?) xmlSerializer.Deserialize(@this);
	}

	/// <summary>
	///     Deserialize XML string, optionally only an inner fragment of the XML, as specified by the innerStartTag parameter.
	/// </summary>
	public static T? FromXml<T>(this string @this, string rootTag, string defaultNamespace = "") {
		using (var stringReader = new StringReader(@this)) return XmlReader.Create(stringReader).FromXml<T>(rootTag, defaultNamespace);
	}

	public static T? FromXml<T>(this Stream @this, string rootTag, string defaultNameSpace = "") => XmlReader.Create(@this).FromXml<T>(rootTag, defaultNameSpace);

	public static string? InnerXml(this XElement element) {
		using (var reader = element?.Parent?.CreateReader()) {
			if (reader == null) return null;
			reader.MoveToContent();
			return reader.ReadInnerXml();
		}
	}
}