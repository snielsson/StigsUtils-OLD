// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.
using System.Reflection;
namespace StigsUtils.Extensions;

public static class AssemblyExtensions {
	// Inspired by this Jon Skeet answer on StackOverflow: : https://stackoverflow.com/a/7889272/193414
	public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly) {
		if (assembly == null) throw new ArgumentNullException(nameof(assembly));
		try { return assembly.GetTypes(); }
		catch (ReflectionTypeLoadException e) {
			return e.Types.Where(t => t != null)!;
		}
	}

	public static IEnumerable<Type> GetInterfaceImplementations<TInterface>(this Assembly @this) => @this.GetInterfaceImplementations(typeof(TInterface));
	public static IEnumerable<Type> GetInterfaceImplementations(this Assembly @this, Type type) {
		if (!type.IsInterface) throw new InvalidOperationException("GetImplementations should be called for an interface type.");
		return @this.GetLoadableTypes().Where(x => x.GetInterfaces().Contains(type));
	}
	public static IEnumerable<Type> GetConcreteSubclasses<T>(this Assembly @this) => @this.GetConcreteSubclasses(typeof(T));
	public static IEnumerable<Type> GetConcreteSubclasses(this Assembly @this, Type type) => @this.GetLoadableTypes().Where(x => x.IsSubclassOf(type) && !x.IsAbstract);
		
		
	public static string ResolveToFullyQualifiedPath(this Assembly @this, string path) {
		if (Path.IsPathFullyQualified(path)) return path;
		var directory = Path.GetDirectoryName(@this.Location);
		var result = Path.Combine(directory??"", path);
		return result;
	}

	public static IEnumerable<Type> GetImplementations<T>(this Assembly @this) => @this.GetImplementations(typeof(T));

	public static IEnumerable<Type> GetImplementations(this Assembly @this, Type type) => @this
		.GetLoadableTypes()
		.Where(x => x.IsAssignableFrom(type));
}