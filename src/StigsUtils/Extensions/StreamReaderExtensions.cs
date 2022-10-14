// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System.Text;
namespace StigsUtils.Extensions;

public static class StreamReaderExtensions {
	public static StreamReader SkipPast(this StreamReader @this, string s,IEqualityComparer<int>? comparer = null) => @this.Read(s, null!, comparer);

	public static string? Read(this StreamReader @this, string s, IEqualityComparer<int>? comparer = null) {
		var sb = new StringBuilder();
		@this.Read(s, sb, comparer);
		if (sb.Length == s.Length) return sb.ToString();
		return null;
	}

	public static string? Read(this StreamReader @this, int count) {
		var sb = new StringBuilder();
		int next;
		for (int i = 0; i < count; i++) {
			if ((next = @this.Read()) == -1) {
				@this.BaseStream.Position -= i;
				return null;
			}
			sb.Append((char)next);
		}
		return sb.ToString();
	}

	public static StreamReader Read(this StreamReader @this, string s, out string? result, IEqualityComparer<int>? comparer = null) {
		var sb = new StringBuilder();
		@this.Read(s, sb, comparer);
		result = sb.Length == s.Length ? sb.ToString() : null; 
		return @this;
	}

	public static StreamReader Read(this StreamReader @this, string s, StringBuilder sb, IEqualityComparer<int>? comparer = null) {
		if (@this.EndOfStream) return @this;
		var i = 0;
		int readCount = 0;
		int next;
		var sbStartLength = sb?.Length ?? 0;
		comparer ??= EqualityComparer<int>.Default;
		while ((next = @this.Read()) > -1) {
			readCount++;
			if (comparer.Equals(next, s[i++])) {
				sb?.Append((char)next);
				if (i == s.Length) return @this;
			}
			else {
				i = 0;
				if(sb!= null && sb.Length > sbStartLength) sb.Remove(sbStartLength, sb.Length-sbStartLength);
			}
		}
		@this.BaseStream.Position -= readCount;
		if(sb!= null && sb.Length > sbStartLength) sb.Remove(sbStartLength, sb.Length-sbStartLength);
		return @this;
	}
}