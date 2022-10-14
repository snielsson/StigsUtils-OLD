// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
namespace StigsUtils.FileUtils;

public class DirPath : AbstractPath {
	public DirPath(string value, params object?[] args) : base(value, args) {
		if (File.Exists(Value)) Value = Path.GetDirectoryName(Value) ?? throw new ArgumentNullException();
	}

	public DirPath(FilePath value) : this(value.FullPath) { }

	public static implicit operator DirPath(string x) => new DirPath(x);

	public static implicit operator string(DirPath x) => x.Value;

	public override bool Exists => Directory.Exists(Value);

	public DirPath DeleteRecursive() => Delete(true);

	public override DirPath Delete() => Delete(false);

	public DirPath Delete(bool recursive) {
		if (Exists) Directory.Delete(Value, recursive);
		return this;
	}

	public static DirPath operator /(string x, DirPath y) => new DirPath(x) / y;

	public static DirPath operator /(DirPath x, string y) => x / new DirPath(y);

	public static FilePath operator +(DirPath x, FilePath y) => x + y.Value;

	public static FilePath operator +(DirPath x, string y) => new(CombineAll(x, y));

	public static DirPath operator /(DirPath x, DirPath y) {
		if (y.IsRelative) return Path.Combine(x.Value, y.Value);
		return Path.Combine(x.Value, Path.GetRelativePath(x.Value, y.Value));
	}


	public bool NotEmpty => !Empty;
	public bool Empty {
		get {
			if (!Exists) throw new ArgumentException($"{Value} is not an existing directory");
			return Directory.EnumerateFileSystemEntries(Value).Any();
		}
	}

	public bool IsEmpty() => Empty;
	public bool IsNotEmpty => NotEmpty;

	public DirPath? Parent {
		get {
			string? parentDir = Directory.GetParent(Value)?.FullName;
			return parentDir == null ? null : new DirPath(parentDir);
		}
	}

	public FilePath? SearchUp(string searchPattern) {
		var result = Directory.EnumerateFiles(Value, searchPattern).FirstOrDefault();
		if (result != null) return result;
		return Parent?.SearchUp(searchPattern);
	}
}