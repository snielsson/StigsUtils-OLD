// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
namespace StigsUtils.FileUtils;

public abstract class AbstractPath {
	public AbstractPath(string value, params object?[] args) {
		Value = value ?? throw new ArgumentNullException(nameof(value));
		if (args.Any()) Value = Path.Combine(value, Path.Combine(args.Select(x => x?.ToString() ?? "").ToArray()));
	}

	public static string CombineAll(string arg, params object?[] args) { 
		if (args.Any()) return Path.Combine(arg, Path.Combine(args.Select(x => x?.ToString() ?? "").ToArray()));
		return arg;
	}

	public string Value { get; protected set; }
//	public FsPath Abs => GetFullPath(Value);
//	public FsPath Rel => RelativeTo(Environment.CurrentDirectory);

//	public FsPath RelativeTo(string path) => GetRelativePath(Value, path);

	// public static implicit operator FsPath(string x) => new(x);
	// public static implicit operator string(FsPath x) => x.Value;
	//
	// public static implicit operator FileInfo(FsPath x) => new(x.Value);
	//
	// public static implicit operator FsPath(FileInfo x) => new(x.FullName);
	//
	// public static implicit operator DirectoryInfo(FsPath x) => new(x.Value);
	//
	// public static implicit operator FsPath(DirectoryInfo x) => new(x.FullName);

	public override string ToString() => Value;

	// public static FsPath operator /(string x, FsPath y) => new FsPath(x) / y;
	// public static FsPath operator /(FsPath x, string y) => x / new FsPath(y);
	// public static FsPath operator /(FsPath x, FsPath y) {
	// 	if (y.IsRelative) return Combine(x.Value, y.Value);
	// 	return Combine(x.Value, GetRelativePath(x.Value, y.Value));
	// }

	public bool IsRelative => !Path.IsPathRooted(Value);
	public virtual bool Exists => File.Exists(Value) || Directory.Exists(Value);
	public virtual bool NotExists => !Exists;
	public AbstractPath ShouldNotExist() => !Exists ? this : throw new ArgumentException($"{this} does not exist.");
	public bool ExistsAsFile => File.Exists(Value);
	public bool ExistsAsDir => Directory.Exists(Value);
	// public IEnumerable<FsPath> EnumerateAll {
	// 	get {
	// 		string? dir = ExistsAsDir ? Value : GetDirectoryName(Value);
	// 		if (dir == null) yield break;
	// 		foreach (var x in new DirectoryInfo(dir).EnumerateFileSystemInfos()) {
	// 			yield return x.FullName;
	// 		}
	// 	}
	// }

	public abstract AbstractPath Delete();
}