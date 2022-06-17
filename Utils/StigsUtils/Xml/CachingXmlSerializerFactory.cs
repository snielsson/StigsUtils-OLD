// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System.Collections.Concurrent;
using System.Globalization;
using System.Xml.Serialization;
namespace StigsUtils.Xml;

/// <summary>
///   A caching factory to avoid memory leaks in the XmlSerializer class.
///   See http://dotnetcodebox.blogspot.dk/2013/01/xmlserializer-class-may-result-in.html
/// </summary>
public static class CachingXmlSerializerFactory {
	private static readonly ConcurrentDictionary<string, XmlSerializer> Cache = new();

	//TEST
	public static XmlSerializer Create(Type type, XmlRootAttribute root) {
		if (type == null) {
			throw new ArgumentNullException(nameof(type));
		}
		if (root == null) {
			throw new ArgumentNullException(nameof(root));
		}
		var key = string.Format(CultureInfo.InvariantCulture, "{0}:{1}", type, root.ElementName);
		return Cache.GetOrAdd(key, _ => new XmlSerializer(type, root));
	}

	//TEST
	public static XmlSerializer Create<T>(XmlRootAttribute root) => Create(typeof(T), root);

	//TEST
	public static XmlSerializer Create<T>() => Create(typeof(T));

	//TEST
	public static XmlSerializer Create<T>(string defaultNamespace) => Create(typeof(T), defaultNamespace);

	//TEST
	public static XmlSerializer Create(Type type) => new(type);

	//TEST
	public static XmlSerializer Create(Type type, string defaultNamespace) => new(type, defaultNamespace);
}