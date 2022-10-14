// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using StigsUtils.DataTypes.Exceptions;
using static System.IO.Path;
namespace StigsUtils.FileUtils; 

public class FilePath : AbstractPath {
	public FilePath(string value, params object?[] args) : base(value, args) { }
	public static implicit operator FilePath(string x) => new(x);
	public static implicit operator string(FilePath x) => x.Value;
	public override bool Exists => File.Exists(Value);
	public static FilePath ExistingFilePath(string path) => new FilePath(path).AssertExists();
	public FilePath AssertExists(string? customMessage = null) {
		if (!Exists) throw this.Error(customMessage ?? $"AssertExists failed for the file {Value}.");
		return this;
	}
	public string FullPath => GetFullPath(Value);

	public override FilePath Delete() {
		if(Exists) File.Delete(Value);
		return this;
	}
	public static FilePath operator /(DirPath x, FilePath y) => new(x,y);

	public FilePath ExistsOr(Action orAction) {
		if(!Exists) orAction();
		return this;
	}
	
	public bool IsReadable() {
		if (!Exists) return false;
		var result = (File.GetAttributes(Value) & FileAttributes.ReadOnly) != 0;
		return result;
	}

	public FilePath SetReadOnly() {
		AssertExists();
		File.SetAttributes(Value, File.GetAttributes(Value) & ~FileAttributes.ReadOnly);
		return this;
	}

	public FilePath UnSetReadOnly() {
		AssertExists();
		File.SetAttributes(Value, File.GetAttributes(Value) ^ ~FileAttributes.ReadOnly);
		return this;
	}

	public FilePath IsReadableOr(Action orAction) {
		if (!IsReadable()) orAction();
		return this;
	}
	public string ReadAllText() => File.ReadAllText(Value);

}