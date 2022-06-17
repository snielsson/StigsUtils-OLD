// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
namespace StigsUtils.Extensions;

public static class StringExtensions {
	public static IEnumerable<long>? ParseLongs(this string @this, char sep = ',') {
		foreach (var x in @this.Split(',')) {
			if (!long.TryParse(x, out var parsed)) throw new ArgumentException($"Could not parse {x} as a long.");
			yield return parsed;
		}
	}

	public static bool TryParseLongs(this string @this, out List<long> result, char sep = ',') {
		var strings = @this.Split(',');
		result = new List<long>(strings.Length);
		foreach (var x in @this.Split(',')) {
			if (!long.TryParse(x, out var parsed)) return false;
			result.Add(parsed);
		}
		return true;
	}
	
		private const char Cr = (char)0x0D;
		private const char Lf = (char)0x0A;

		// Copied from decompiled Microsoft.AspNet.Identity.Crypto
		private const int Pbkdf2IterCount = 1000;

		private const int Pbkdf2SubkeyLength = 32;
		private const int SaltSize = 16;
		private static readonly Lazy<char[]> _invalidFileNameChars = new Lazy<char[]>(Path.GetInvalidFileNameChars);

		public static readonly string[] _dateTimeFormats = {
			"yyyyMMdd"
		};

		public static bool HasValue(this string @this) {
			return !string.IsNullOrEmpty(@this);
		}

		/// <summary>
		///     LocalizeFunc should be set to a func doing the actual localization.
		/// </summary>
		public static Func<string, string, string> LocalizeFunc { get; set; } = (s, _) => s;
		public static string L(this string @this, string? locale = null) => LocalizeFunc(@this, locale ?? "");
		public static string NotNull(this string @this) {
			if(@this == null) throw new ArgumentException("String is null.");
			return @this;
		}
		public static string NotEmpty(this string @this) {
			if (@this.NotNull().Length == 0) throw new ArgumentException("String is empty.");
			return @this;
		}
		public static bool IsNullOrEmpty(this string @this) => string.IsNullOrEmpty(@this);
		public static bool IsNullOrWhitespace(this string @this) => string.IsNullOrWhiteSpace(@this);
		public static bool IsNotNullOrEmpty(this string @this) => !string.IsNullOrEmpty(@this);
		public static bool IsNotNullOrWhitespace(this string @this) => !string.IsNullOrWhiteSpace(@this);

		public static string ToValidPath(this string @this, string? spaceReplaceValue = null, string? replaceValue = null)
		{
			var sb = new StringBuilder(@this.Length);
			char[] invalidChars = _invalidFileNameChars.Value;
			foreach (var ch in @this)
			{
				if (Array.IndexOf(invalidChars, ch) >= 0)
				{
					if (replaceValue != null) sb.Append(replaceValue);
				}
				else
				{
					if (ch == ' ' && spaceReplaceValue != null) sb.Append(spaceReplaceValue);
					else sb.Append(ch);
				}
			}
			return sb.ToString();
		}

		public static string EnsureDirExists(this string @this)
		{
			if (!Directory.Exists(@this)) Directory.CreateDirectory(@this);
			return @this;
		}

		public static string EnsureDirIsWritable(this string @this)
		{
			File.SetAttributes(@this.EnsureDirExists(), File.GetAttributes(@this) & ~FileAttributes.ReadOnly);
			return @this;
		}

		public static string EnsureIsExistingFile(this string @this)
		{
			if (!File.Exists(@this)) throw new ArgumentException($"{@this} does not exist.");
			return @this;
		}

		public static string EnsureIsWritableFile(this string @this)
		{
			if (!File.Exists(@this)) throw new ArgumentException($"{@this} does not exist.");
			if (new FileInfo(@this).IsReadOnly) throw new ArgumentException($"{@this} is read only.");
			;
			return @this;
		}

		public static FileSystemInfo? ToFileSystemInfo(this string @this)
		{
			if (File.Exists(@this)) return new FileInfo(@this);
			if (Directory.Exists(@this)) return new DirectoryInfo(@this);
			return null;
		}

		public static string EnsureWritableDirectory(this string @this)
		{
			var dir = Path.GetDirectoryName(@this);
			if (!Directory.Exists(dir)) throw new ArgumentException($"{dir} is not an existing directory.");
			if (new DirectoryInfo(dir).Attributes.HasFlag(FileAttributes.ReadOnly)) throw new ArgumentException($"{dir} is a read only directory.");
			;
			return @this;
		}

		public static string Combine(this string @this, params string[] args)
		{
			if (args == null || args.Length == 0) return @this;
			var result = Path.Combine(@this, Path.Combine(args));
			return result;
		}

		public static string CombineToExistingDir(this string @this, params string[] args)
		{
			if (args == null || args.Length == 0) return @this;
			var result = Path.Combine(@this, Path.Combine(args));
			var dir = Path.GetDirectoryName(result);
			if (!Directory.Exists(dir)) throw new Exception(dir + " is not an existing directory.");
			return result;
		}

		public static string CombineToExistingDirOrCreate(this string @this, params string[] args)
		{
			if (args == null || args.Length == 0) return @this;
			var result = Path.Combine(@this, Path.Combine(args));
			var dir = Path.GetDirectoryName(result) ?? throw new ArgumentNullException("Path.GetDirectoryName(result)");
			if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
			return result;
		}

		public static string CreateDirIfNotExists(this string @this)
		{
			if (!Directory.Exists(@this)) Directory.CreateDirectory(@this);
			return @this;
		}

		public static string CombineToExistingFile(this string @this, params string[] args)
		{
			if (args == null || args.Length == 0) return @this;
			var result = Path.Combine(@this, Path.Combine(args));
			if (!File.Exists(result)) throw new Exception(result + " is not an existing file.");
			return result;
		}

		public static string RemoveWhiteSpace(this string @this)
		{
			var result = new char[@this.Length];
			var whitespaceCount = 0;
			for (var i = 0; i < result.Length; i++)
			{
				var ch = @this[i];
				if (char.IsWhiteSpace(ch))
				{
					whitespaceCount++;
					continue;
				}
				result[i - whitespaceCount] = ch;
			}
			return new string(result, 0, @this.Length - whitespaceCount);
		}

		public static IEnumerable<string> SubStrings(this string @this, string delimiter, int startPos = 0, bool includeEmpty = true,
			int length = int.MaxValue)
		{
			if (string.IsNullOrEmpty(delimiter)) throw new ArgumentNullException($"{nameof(delimiter)} argument is null or empty");
			if (startPos < 0) throw new ArgumentOutOfRangeException(nameof(startPos));
			if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
			var endPos = startPos + Math.Min(@this.Length - startPos, length);
			var delimiterPos = 0;
			for (var i = startPos; i < endPos; i++)
			{
				delimiterPos = @this[i] == delimiter[delimiterPos] ? delimiterPos + 1 : 0;
				if (delimiterPos == delimiter.Length)
				{
					var result = @this.Substring(startPos, i - delimiter.Length - startPos + 1);
					if (includeEmpty || result.Length > 0) yield return result;

					startPos = i + 1;
					delimiterPos = 0;
				}
			}
			if (startPos <= endPos)
			{
				var result = @this.Substring(startPos, endPos - startPos);
				if (includeEmpty || result.Length > 0) yield return result;
			}
		}

		/// <summary>
		///     Load string as the filepath of a text file.
		/// </summary>
		/// <returns>Text contents of the file at the filepath.</returns>
		public static string Load(this string @this) => File.ReadAllText(@this);

		public static T ParseEnum<T>(this string @this, bool ignoreCase = false)
		{
			var result = Enum.Parse(typeof(T), @this, ignoreCase);
			if (result is T) return (T)result;
			throw new ArgumentException(@this + " is not a value of enumeration " + typeof(T));
		}

		public static bool TryParseEnum<T>(this string @this, bool ignoreCase, out T? result)
		{
			if (Enum.TryParse(typeof(T), @this, ignoreCase, out var obj))
			{
				if (obj is T)
				{
					result = (T)obj;
					return true;
				}
			}
			result = default;
			return false;
		}

		public static string Render(this string @this, params (string key, string val)[] args)
		{
			var sb = new StringBuilder(@this);
			foreach (var (key, val) in args) sb.Replace(key, val);
			return sb.ToString();
		}

		[DebuggerStepThrough]
		public static string Render(this string @this, object args, string openingDelimiter = "{{", string closingDelimiter = "}}")
		{
			var sb = new StringBuilder(@this);
			foreach (var prop in args.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)) sb.Replace(openingDelimiter + prop.Name + closingDelimiter, prop.GetValue(args, null)?.ToString());
			return sb.ToString();
		}

		public static bool IsFile(this string @this) => File.Exists(@this);


		public static IEnumerable<string> Files(this string @this, string? pattern = null, bool recursive = false)
		{
			var fullPath = Path.GetFullPath(@this);
			return Directory.EnumerateFiles(fullPath, pattern!, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
		}

		public static string TryConvertFromBase64(this string @this)
		{
			try
			{
				byte[] base64EncodedBytes = Convert.FromBase64String(@this);
				return Encoding.UTF8.GetString(base64EncodedBytes);
			}
			catch (FormatException)
			{
				return @this;
			}
		}
		public static string FromBase64(this string @this)
		{
			byte[] base64EncodedBytes = Convert.FromBase64String(@this);
			return Encoding.UTF8.GetString(base64EncodedBytes);
		}
		public static string ToBase64(this string @this)
		{
			byte[] plainTextBytes = Encoding.UTF8.GetBytes(@this);
			return Convert.ToBase64String(plainTextBytes);
		}
		public static string Format(this string @this, params object[] args) => string.Format(CultureInfo.CurrentCulture, @this, args);
		public static IEnumerable<string> AsLines(this string @this)
		{
			var lineStart = 0;
			for (var i = 0; i < @this.Length; i++)
			{
				var ch = @this[i];
				if (ch == Lf)
				{
					var lineEnd = i;
					if (i > 0 && @this[i - 1] == Cr) lineEnd--;
					yield return @this.Substring(lineStart, lineEnd - lineStart);
					lineStart = i + 1;
				}
			}
		}
		public static byte[] ToBytes(this string @this, Encoding? encoding = null) => (encoding ?? Encoding.UTF8).GetBytes(@this);
		public static DateTime? AsDate(this string @this, string format = "yyyy-MM-dd", IFormatProvider? formatProvider = null)
		{
			try
			{
				return DateTime.ParseExact(@this, format, formatProvider ?? CultureInfo.InvariantCulture);
			}
			catch (FormatException)
			{
				return null;
			}
		}
		public static double? AsDouble(this string @this, NumberStyles numberStyles, IFormatProvider? formatProvider = null)
		{
			var cultureInfo = formatProvider ?? CultureInfo.InvariantCulture;
			if (double.TryParse(@this, numberStyles, cultureInfo, out var result)) return result;
			return null;
		}
		public static double? AsDouble(this string @this)
		{
			if (double.TryParse(@this, out var result)) return result;
			return null;
		}
		public static long? AsLong(this string @this)
		{
			if (long.TryParse(@this, out var result)) return result;
			return null;
		}
		public static DateTime ToDate(this string @this, IFormatProvider? formatProvider = null)
		{
			if (DateTime.TryParseExact(@this, _dateTimeFormats, formatProvider ?? CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var dateTime)) return dateTime;
			throw new ArgumentException($"Could not parse {@this} as a DateTime using these formats:\n{_dateTimeFormats.ToPrettyJson()}");
		}
		public static DateTime? TryParseDateTime(this string @this)
		{
			if (!DateTime.TryParse(@this, out var result)) return null;
			return result;
		}
		public static DateTime? TryParseDateTime(this string @this, string format, IFormatProvider? formatProvider = null,
			DateTimeStyles styles = default(DateTimeStyles))
		{
			formatProvider = formatProvider ?? CultureInfo.InvariantCulture;
			if (!DateTime.TryParseExact(@this, format, formatProvider, styles, out var result)) return null;
			return result;
		}

		public static string InQuotes(this string @this) => $@"""{@this.Trim('"')}""";

		public static StringBuilder Build(this IEnumerable<string> @this)
		{
			var sb = new StringBuilder();
			foreach (var str in @this) sb.Append(str);
			return sb;
		}

		public static string BuildString(this IEnumerable<string> @this) => @this.Build().ToString();

		public static bool LessThan(this string @this, string comparand)
		{
			var result = string.Compare(@this, comparand, StringComparison.Ordinal);
			return result < 0;
		}

		public static bool LessThanOrEqual(this string @this, string comparand)
		{
			var result = string.Compare(@this, comparand, StringComparison.Ordinal);
			return result <= 0;
		}

		public static bool GreaterThan(this string @this, string comparand)
		{
			var result = string.Compare(@this, comparand, StringComparison.Ordinal);
			return result > 0;
		}

		public static bool GreaterThanOrEqual(this string @this, string comparand)
		{
			var result = string.Compare(@this, comparand, StringComparison.Ordinal);
			return result >= 0;
		}

		public static string HashPassword(this string @this)
		{
			byte[] salt;
			byte[] bytes;
			using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(@this, 16, 1000))
			{
				salt = rfc2898DeriveBytes.Salt;
				bytes = rfc2898DeriveBytes.GetBytes(32);
			}
			var inArray = new byte[49];
			Buffer.BlockCopy(salt, 0, inArray, 1, 16);
			Buffer.BlockCopy(bytes, 0, inArray, 17, 32);
			return Convert.ToBase64String(inArray);
		}

		public static bool VerifyHashedPassword(this string @this, string password)
		{
			if (password == null) throw new ArgumentNullException(nameof(password));
			byte[] numArray = Convert.FromBase64String(@this);
			if (numArray.Length != 49 || numArray[0] != 0) return false;
			var salt = new byte[16];
			Buffer.BlockCopy(numArray, 1, salt, 0, 16);
			var a = new byte[32];
			Buffer.BlockCopy(numArray, 17, a, 0, 32);
			byte[] bytes;
			using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, 1000)) bytes = rfc2898DeriveBytes.GetBytes(32);
			return ByteArraysEqual(a, bytes);
		}

		[MethodImpl(MethodImplOptions.NoOptimization)]
		private static bool ByteArraysEqual(byte[] a, byte[] b)
		{
			if (ReferenceEquals(a, b)) return true;
			if (a == null || b == null || a.Length != b.Length) return false;
			var flag = true;
			for (var index = 0; index < a.Length; ++index) flag = flag & (a[index] == b[index]);
			return flag;
		}

		//copied to StigsUtils
		public static string TrimEnd(this string @this, string str)
		{
			if (@this.Length < str.Length) return @this;
			var j = @this.Length;
			for (var i = str.Length - 1; i >= 0; i--)
			{
				j--;
				if (str[i] != @this[j]) break;
			}
			return @this.Substring(0, j);
		}

		private static readonly Regex StripWhiteSpaceRegex = new Regex(@"\s", RegexOptions.Compiled);
		public static string StripWhiteSpace(this string @this)
		{
			var result = StripWhiteSpaceRegex.Replace(@this, string.Empty);
			return result;
		}	
}