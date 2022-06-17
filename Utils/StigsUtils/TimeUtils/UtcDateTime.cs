// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System.Text.Json;
using Newtonsoft.Json;
using StigsUtils.Extensions;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;
namespace StigsUtils.TimeUtils;

[JsonConverter(typeof(NewtonsoftJsonConverter))]
[System.Text.Json.Serialization.JsonConverter(typeof(JsonConverter))]
public struct UtcDateTime : IComparable<UtcDateTime>, IComparable, IEquatable<UtcDateTime> {
	// Statics
	public static UtcDateTime MaxValue => new(DateTime.MaxValue);
	public static UtcDateTime MinValue => new(DateTime.MinValue);
	public static readonly UtcDateTime Default = MinValue;
	public static UtcDateTime UtcNow => UtcNowProvider();
	public static UtcDateTime Today => UtcNowProvider().Date;
	public static Func<UtcDateTime> UtcNowProvider { get; set; } = () => DateTime.UtcNow;
	public static string DefaultFormat { get; set; } = "O";
	public static int SizeInBytes { get; } = sizeof(long);

	// Ctors
	private UtcDateTime(long ticks) => Value = new DateTime(ticks, DateTimeKind.Utc);

	public UtcDateTime(DateTime dateTime) => Value = new DateTime(dateTime.Ticks, DateTimeKind.Utc);

  public UtcDateTime(int year, int month = 1, int day = 1, int hour = 0, int minute = 0, int second = 0) => Value = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);

  // Properties
	public DateTime Value { get; }

	//Conversion methods:
	public DateTime ToLocalTime(TimeZoneInfo timeZoneInfo) {
		var dt = new DateTime(TimeZoneInfo.ConvertTimeFromUtc(Value, timeZoneInfo).Ticks, DateTimeKind.Local);
		return dt;
	}

	public DateTime ToLocalTime(string s) => new(TimeZoneInfo.ConvertTimeFromUtc(Value, s.ToTimeZoneInfo()).Ticks, DateTimeKind.Local);

	public override string ToString() => ToString(DefaultFormat);

	public string ToString(string format, IFormatProvider? formatProvider = null) => Value.ToString(format, formatProvider);

	public string ToString(string format) => Value.ToString(format);

	public int ToUnixTime() => Value.ToUnixTime();

  // Time manipulation
  private static UtcDateTime? _now;
  public static UtcDateTime Now { get => _now ?? DateTime.UtcNow; }
  public static UtcDateTime Set(UtcDateTime? val) {
    _now = val;
    return Now;
  }

	// Readers
	public long Ticks => Value.Ticks;
	public UtcDateTime Date => Value.Date;
	public int Day => Value.Day;
	public DateTimeKind Kind => Value.Kind;
	public DayOfWeek DayOfWeek => Value.DayOfWeek;
	public TimeSpan TimeOfDay => Value.TimeOfDay;
	public int Year => Value.Year;
	public int Month => Value.Month;
	public DateTime ToDateTime => Value;

  
	// Modifiers
	public UtcDateTime Add(TimeSpan x) => Value.Add(x);

	public UtcDateTime AddYears(int x) => Value.AddYears(x);

	public UtcDateTime AddMonths(int x) => Value.AddMonths(x);

	public UtcDateTime AddDays(double x) => Value.AddDays(x);

	public UtcDateTime AddHours(double x) => Value.AddHours(x);

	public UtcDateTime AddMinutes(double x) => Value.AddMinutes(x);

	public UtcDateTime AddSeconds(double x) => Value.AddSeconds(x);

	public UtcDateTime AddMilliseconds(double x) => Value.AddMilliseconds(x);

	public UtcDateTime AddTicks(long x) => Value.AddTicks(x);

	public UtcDateTime Truncate(in long truncateBy) {
		var ticks = Value.Ticks / truncateBy * truncateBy;
		return new UtcDateTime(ticks);
	}

	// Comparison & Equality
	public int CompareTo(UtcDateTime other) => Value.CompareTo(other.Value);

	public int CompareTo(object? obj) {
		if (ReferenceEquals(null, obj)) return 1;
		return obj is UtcDateTime other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(UtcDateTime)}");
	}

	public bool Equals(UtcDateTime other) => Value.Equals(other.Value);

	public override bool Equals(object? obj) => obj is UtcDateTime other && Equals(other);

	public override int GetHashCode() => Value.GetHashCode();

	// Operators
	public static implicit operator DateTime(UtcDateTime x) => x.Value;

	public static implicit operator UtcDateTime(long x) => new(x);

	public static implicit operator UtcDateTime(DateTime x) => new(x);

	public static implicit operator UtcDateTime(string x) => new(DateTime.Parse(x));

	public static TimeSpan operator -(UtcDateTime lhs, UtcDateTime rhs) => new(lhs.Value.Ticks - rhs.Value.Ticks);

	public static TimeSpan? operator -(UtcDateTime? lhs, UtcDateTime? rhs) {
		if (lhs == null) return null;
		if (rhs == null) return null;
		return new TimeSpan(lhs.Value.Value.Ticks - rhs.Value.Value.Ticks);
	}

	public static UtcDateTime operator +(UtcDateTime left, TimeSpan right) => new(left.Value + right);

	public static UtcDateTime operator -(UtcDateTime left, TimeSpan right) => new(left.Value - right);

	public static bool operator <(UtcDateTime left, UtcDateTime right) => left.CompareTo(right) < 0;

	public static bool operator >(UtcDateTime left, UtcDateTime right) => left.CompareTo(right) > 0;

	public static bool operator <=(UtcDateTime left, UtcDateTime right) => left.CompareTo(right) <= 0;

	public static bool operator >=(UtcDateTime left, UtcDateTime right) => left.CompareTo(right) >= 0;

	public static bool operator ==(UtcDateTime left, UtcDateTime right) => left.Equals(right);

	public static bool operator !=(UtcDateTime left, UtcDateTime right) => !left.Equals(right);

	// Json converter
	public class JsonConverter : System.Text.Json.Serialization.JsonConverter<UtcDateTime> {
		public override UtcDateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => reader.GetDateTime();

		public override void Write(Utf8JsonWriter writer, UtcDateTime value, JsonSerializerOptions options) {
			writer.WriteStringValue(value);
		}
	}

	public class NewtonsoftJsonConverter : Newtonsoft.Json.JsonConverter {
		public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) {
			if (value == null) writer.WriteNull();
			else writer.WriteRawValue(((UtcDateTime)value).Value.ToJson());
		}

		public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer) => new UtcDateTime((DateTime)reader.Value!);

		public override bool CanConvert(Type objectType) => objectType == typeof(UtcDateTime);
	}
}

/*
      // TimeZones: 
[
{
  "Id": "Dateline Standard Time",
  "DisplayName": "(UTC-12:00) International Date Line West",
  "StandardName": "Dateline Standard Time",
  "DaylightName": "Dateline Daylight Time",
  "BaseUtcOffset": "-12:00:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "UTC-11",
  "DisplayName": "(UTC-11:00) Coordinated Universal Time-11",
  "StandardName": "UTC-11",
  "DaylightName": "UTC-11",
  "BaseUtcOffset": "-11:00:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "Aleutian Standard Time",
  "DisplayName": "(UTC-10:00) Aleutian Islands",
  "StandardName": "Aleutian Standard Time",
  "DaylightName": "Aleutian Daylight Time",
  "BaseUtcOffset": "-10:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Hawaiian Standard Time",
  "DisplayName": "(UTC-10:00) Hawaii",
  "StandardName": "Hawaiian Standard Time",
  "DaylightName": "Hawaiian Daylight Time",
  "BaseUtcOffset": "-10:00:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "Marquesas Standard Time",
  "DisplayName": "(UTC-09:30) Marquesas Islands",
  "StandardName": "Marquesas Standard Time",
  "DaylightName": "Marquesas Daylight Time",
  "BaseUtcOffset": "-09:30:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "Alaskan Standard Time",
  "DisplayName": "(UTC-09:00) Alaska",
  "StandardName": "Alaskan Standard Time",
  "DaylightName": "Alaskan Daylight Time",
  "BaseUtcOffset": "-09:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "UTC-09",
  "DisplayName": "(UTC-09:00) Coordinated Universal Time-09",
  "StandardName": "UTC-09",
  "DaylightName": "UTC-09",
  "BaseUtcOffset": "-09:00:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "Pacific Standard Time (Mexico)",
  "DisplayName": "(UTC-08:00) Baja California",
  "StandardName": "Pacific Standard Time (Mexico)",
  "DaylightName": "Pacific Daylight Time (Mexico)",
  "BaseUtcOffset": "-08:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "UTC-08",
  "DisplayName": "(UTC-08:00) Coordinated Universal Time-08",
  "StandardName": "UTC-08",
  "DaylightName": "UTC-08",
  "BaseUtcOffset": "-08:00:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "Pacific Standard Time",
  "DisplayName": "(UTC-08:00) Pacific Time (US & Canada)",
  "StandardName": "Pacific Standard Time",
  "DaylightName": "Pacific Daylight Time",
  "BaseUtcOffset": "-08:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "US Mountain Standard Time",
  "DisplayName": "(UTC-07:00) Arizona",
  "StandardName": "US Mountain Standard Time",
  "DaylightName": "US Mountain Daylight Time",
  "BaseUtcOffset": "-07:00:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "Mountain Standard Time (Mexico)",
  "DisplayName": "(UTC-07:00) Chihuahua, La Paz, Mazatlan",
  "StandardName": "Mountain Standard Time (Mexico)",
  "DaylightName": "Mountain Daylight Time (Mexico)",
  "BaseUtcOffset": "-07:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Mountain Standard Time",
  "DisplayName": "(UTC-07:00) Mountain Time (US & Canada)",
  "StandardName": "Mountain Standard Time",
  "DaylightName": "Mountain Daylight Time",
  "BaseUtcOffset": "-07:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Central America Standard Time",
  "DisplayName": "(UTC-06:00) Central America",
  "StandardName": "Central America Standard Time",
  "DaylightName": "Central America Daylight Time",
  "BaseUtcOffset": "-06:00:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "Central Standard Time",
  "DisplayName": "(UTC-06:00) Central Time (US & Canada)",
  "StandardName": "Central Standard Time",
  "DaylightName": "Central Daylight Time",
  "BaseUtcOffset": "-06:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Easter Island Standard Time",
  "DisplayName": "(UTC-06:00) Easter Island",
  "StandardName": "Easter Island Standard Time",
  "DaylightName": "Easter Island Daylight Time",
  "BaseUtcOffset": "-06:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Central Standard Time (Mexico)",
  "DisplayName": "(UTC-06:00) Guadalajara, Mexico City, Monterrey",
  "StandardName": "Central Standard Time (Mexico)",
  "DaylightName": "Central Daylight Time (Mexico)",
  "BaseUtcOffset": "-06:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Canada Central Standard Time",
  "DisplayName": "(UTC-06:00) Saskatchewan",
  "StandardName": "Canada Central Standard Time",
  "DaylightName": "Canada Central Daylight Time",
  "BaseUtcOffset": "-06:00:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "SA Pacific Standard Time",
  "DisplayName": "(UTC-05:00) Bogota, Lima, Quito, Rio Branco",
  "StandardName": "SA Pacific Standard Time",
  "DaylightName": "SA Pacific Daylight Time",
  "BaseUtcOffset": "-05:00:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "Eastern Standard Time (Mexico)",
  "DisplayName": "(UTC-05:00) Chetumal",
  "StandardName": "Eastern Standard Time (Mexico)",
  "DaylightName": "Eastern Daylight Time (Mexico)",
  "BaseUtcOffset": "-05:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Eastern Standard Time",
  "DisplayName": "(UTC-05:00) Eastern Time (US & Canada)",
  "StandardName": "Eastern Standard Time",
  "DaylightName": "Eastern Daylight Time",
  "BaseUtcOffset": "-05:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Haiti Standard Time",
  "DisplayName": "(UTC-05:00) Haiti",
  "StandardName": "Haiti Standard Time",
  "DaylightName": "Haiti Daylight Time",
  "BaseUtcOffset": "-05:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Cuba Standard Time",
  "DisplayName": "(UTC-05:00) Havana",
  "StandardName": "Cuba Standard Time",
  "DaylightName": "Cuba Daylight Time",
  "BaseUtcOffset": "-05:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "US Eastern Standard Time",
  "DisplayName": "(UTC-05:00) Indiana (East)",
  "StandardName": "US Eastern Standard Time",
  "DaylightName": "US Eastern Daylight Time",
  "BaseUtcOffset": "-05:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Paraguay Standard Time",
  "DisplayName": "(UTC-04:00) Asuncion",
  "StandardName": "Paraguay Standard Time",
  "DaylightName": "Paraguay Daylight Time",
  "BaseUtcOffset": "-04:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Atlantic Standard Time",
  "DisplayName": "(UTC-04:00) Atlantic Time (Canada)",
  "StandardName": "Atlantic Standard Time",
  "DaylightName": "Atlantic Daylight Time",
  "BaseUtcOffset": "-04:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Venezuela Standard Time",
  "DisplayName": "(UTC-04:00) Caracas",
  "StandardName": "Venezuela Standard Time",
  "DaylightName": "Venezuela Daylight Time",
  "BaseUtcOffset": "-04:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Central Brazilian Standard Time",
  "DisplayName": "(UTC-04:00) Cuiaba",
  "StandardName": "Central Brazilian Standard Time",
  "DaylightName": "Central Brazilian Daylight Time",
  "BaseUtcOffset": "-04:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "SA Western Standard Time",
  "DisplayName": "(UTC-04:00) Georgetown, La Paz, Manaus, San Juan",
  "StandardName": "SA Western Standard Time",
  "DaylightName": "SA Western Daylight Time",
  "BaseUtcOffset": "-04:00:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "Pacific SA Standard Time",
  "DisplayName": "(UTC-04:00) Santiago",
  "StandardName": "Pacific SA Standard Time",
  "DaylightName": "Pacific SA Daylight Time",
  "BaseUtcOffset": "-04:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Turks And Caicos Standard Time",
  "DisplayName": "(UTC-04:00) Turks and Caicos",
  "StandardName": "Turks and Caicos Standard Time",
  "DaylightName": "Turks and Caicos Daylight Time",
  "BaseUtcOffset": "-04:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Newfoundland Standard Time",
  "DisplayName": "(UTC-03:30) Newfoundland",
  "StandardName": "Newfoundland Standard Time",
  "DaylightName": "Newfoundland Daylight Time",
  "BaseUtcOffset": "-03:30:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Tocantins Standard Time",
  "DisplayName": "(UTC-03:00) Araguaina",
  "StandardName": "Tocantins Standard Time",
  "DaylightName": "Tocantins Daylight Time",
  "BaseUtcOffset": "-03:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "E. South America Standard Time",
  "DisplayName": "(UTC-03:00) Brasilia",
  "StandardName": "E. South America Standard Time",
  "DaylightName": "E. South America Daylight Time",
  "BaseUtcOffset": "-03:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "SA Eastern Standard Time",
  "DisplayName": "(UTC-03:00) Cayenne, Fortaleza",
  "StandardName": "SA Eastern Standard Time",
  "DaylightName": "SA Eastern Daylight Time",
  "BaseUtcOffset": "-03:00:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "Argentina Standard Time",
  "DisplayName": "(UTC-03:00) City of Buenos Aires",
  "StandardName": "Argentina Standard Time",
  "DaylightName": "Argentina Daylight Time",
  "BaseUtcOffset": "-03:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Greenland Standard Time",
  "DisplayName": "(UTC-03:00) Greenland",
  "StandardName": "Greenland Standard Time",
  "DaylightName": "Greenland Daylight Time",
  "BaseUtcOffset": "-03:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Montevideo Standard Time",
  "DisplayName": "(UTC-03:00) Montevideo",
  "StandardName": "Montevideo Standard Time",
  "DaylightName": "Montevideo Daylight Time",
  "BaseUtcOffset": "-03:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Saint Pierre Standard Time",
  "DisplayName": "(UTC-03:00) Saint Pierre and Miquelon",
  "StandardName": "Saint Pierre Standard Time",
  "DaylightName": "Saint Pierre Daylight Time",
  "BaseUtcOffset": "-03:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Bahia Standard Time",
  "DisplayName": "(UTC-03:00) Salvador",
  "StandardName": "Bahia Standard Time",
  "DaylightName": "Bahia Daylight Time",
  "BaseUtcOffset": "-03:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "UTC-02",
  "DisplayName": "(UTC-02:00) Coordinated Universal Time-02",
  "StandardName": "UTC-02",
  "DaylightName": "UTC-02",
  "BaseUtcOffset": "-02:00:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "Mid-Atlantic Standard Time",
  "DisplayName": "(UTC-02:00) Mid-Atlantic - Old",
  "StandardName": "Mid-Atlantic Standard Time",
  "DaylightName": "Mid-Atlantic Daylight Time",
  "BaseUtcOffset": "-02:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Azores Standard Time",
  "DisplayName": "(UTC-01:00) Azores",
  "StandardName": "Azores Standard Time",
  "DaylightName": "Azores Daylight Time",
  "BaseUtcOffset": "-01:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Cape Verde Standard Time",
  "DisplayName": "(UTC-01:00) Cabo Verde Is.",
  "StandardName": "Cabo Verde Standard Time",
  "DaylightName": "Cabo Verde Daylight Time",
  "BaseUtcOffset": "-01:00:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "UTC",
  "DisplayName": "(UTC) Coordinated Universal Time",
  "StandardName": "Coordinated Universal Time",
  "DaylightName": "Coordinated Universal Time",
  "BaseUtcOffset": "00:00:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "Morocco Standard Time",
  "DisplayName": "(UTC+00:00) Casablanca",
  "StandardName": "Morocco Standard Time",
  "DaylightName": "Morocco Daylight Time",
  "BaseUtcOffset": "00:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "GMT Standard Time",
  "DisplayName": "(UTC+00:00) Dublin, Edinburgh, Lisbon, London",
  "StandardName": "GMT Standard Time",
  "DaylightName": "GMT Daylight Time",
  "BaseUtcOffset": "00:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Greenwich Standard Time",
  "DisplayName": "(UTC+00:00) Monrovia, Reykjavik",
  "StandardName": "Greenwich Standard Time",
  "DaylightName": "Greenwich Daylight Time",
  "BaseUtcOffset": "00:00:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "W. Europe Standard Time",
  "DisplayName": "(UTC+01:00) Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna",
  "StandardName": "W. Europe Standard Time",
  "DaylightName": "W. Europe Daylight Time",
  "BaseUtcOffset": "01:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Central Europe Standard Time",
  "DisplayName": "(UTC+01:00) Belgrade, Bratislava, Budapest, Ljubljana, Prague",
  "StandardName": "Central Europe Standard Time",
  "DaylightName": "Central Europe Daylight Time",
  "BaseUtcOffset": "01:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Romance Standard Time",
  "DisplayName": "(UTC+01:00) Brussels, Copenhagen, Madrid, Paris",
  "StandardName": "Romance Standard Time",
  "DaylightName": "Romance Daylight Time",
  "BaseUtcOffset": "01:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Central European Standard Time",
  "DisplayName": "(UTC+01:00) Sarajevo, Skopje, Warsaw, Zagreb",
  "StandardName": "Central European Standard Time",
  "DaylightName": "Central European Daylight Time",
  "BaseUtcOffset": "01:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "W. Central Africa Standard Time",
  "DisplayName": "(UTC+01:00) West Central Africa",
  "StandardName": "W. Central Africa Standard Time",
  "DaylightName": "W. Central Africa Daylight Time",
  "BaseUtcOffset": "01:00:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "Namibia Standard Time",
  "DisplayName": "(UTC+01:00) Windhoek",
  "StandardName": "Namibia Standard Time",
  "DaylightName": "Namibia Daylight Time",
  "BaseUtcOffset": "01:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Jordan Standard Time",
  "DisplayName": "(UTC+02:00) Amman",
  "StandardName": "Jordan Standard Time",
  "DaylightName": "Jordan Daylight Time",
  "BaseUtcOffset": "02:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "GTB Standard Time",
  "DisplayName": "(UTC+02:00) Athens, Bucharest",
  "StandardName": "GTB Standard Time",
  "DaylightName": "GTB Daylight Time",
  "BaseUtcOffset": "02:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Middle East Standard Time",
  "DisplayName": "(UTC+02:00) Beirut",
  "StandardName": "Middle East Standard Time",
  "DaylightName": "Middle East Daylight Time",
  "BaseUtcOffset": "02:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Egypt Standard Time",
  "DisplayName": "(UTC+02:00) Cairo",
  "StandardName": "Egypt Standard Time",
  "DaylightName": "Egypt Daylight Time",
  "BaseUtcOffset": "02:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "E. Europe Standard Time",
  "DisplayName": "(UTC+02:00) Chisinau",
  "StandardName": "E. Europe Standard Time",
  "DaylightName": "E. Europe Daylight Time",
  "BaseUtcOffset": "02:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Syria Standard Time",
  "DisplayName": "(UTC+02:00) Damascus",
  "StandardName": "Syria Standard Time",
  "DaylightName": "Syria Daylight Time",
  "BaseUtcOffset": "02:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "West Bank Standard Time",
  "DisplayName": "(UTC+02:00) Gaza, Hebron",
  "StandardName": "West Bank Gaza Standard Time",
  "DaylightName": "West Bank Gaza Daylight Time",
  "BaseUtcOffset": "02:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "South Africa Standard Time",
  "DisplayName": "(UTC+02:00) Harare, Pretoria",
  "StandardName": "South Africa Standard Time",
  "DaylightName": "South Africa Daylight Time",
  "BaseUtcOffset": "02:00:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "FLE Standard Time",
  "DisplayName": "(UTC+02:00) Helsinki, Kyiv, Riga, Sofia, Tallinn, Vilnius",
  "StandardName": "FLE Standard Time",
  "DaylightName": "FLE Daylight Time",
  "BaseUtcOffset": "02:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Turkey Standard Time",
  "DisplayName": "(UTC+02:00) Istanbul",
  "StandardName": "Turkey Standard Time",
  "DaylightName": "Turkey Daylight Time",
  "BaseUtcOffset": "02:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Israel Standard Time",
  "DisplayName": "(UTC+02:00) Jerusalem",
  "StandardName": "Jerusalem Standard Time",
  "DaylightName": "Jerusalem Daylight Time",
  "BaseUtcOffset": "02:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Kaliningrad Standard Time",
  "DisplayName": "(UTC+02:00) Kaliningrad",
  "StandardName": "Russia TZ 1 Standard Time",
  "DaylightName": "Russia TZ 1 Daylight Time",
  "BaseUtcOffset": "02:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Libya Standard Time",
  "DisplayName": "(UTC+02:00) Tripoli",
  "StandardName": "Libya Standard Time",
  "DaylightName": "Libya Daylight Time",
  "BaseUtcOffset": "02:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Arabic Standard Time",
  "DisplayName": "(UTC+03:00) Baghdad",
  "StandardName": "Arabic Standard Time",
  "DaylightName": "Arabic Daylight Time",
  "BaseUtcOffset": "03:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Arab Standard Time",
  "DisplayName": "(UTC+03:00) Kuwait, Riyadh",
  "StandardName": "Arab Standard Time",
  "DaylightName": "Arab Daylight Time",
  "BaseUtcOffset": "03:00:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "Belarus Standard Time",
  "DisplayName": "(UTC+03:00) Minsk",
  "StandardName": "Belarus Standard Time",
  "DaylightName": "Belarus Daylight Time",
  "BaseUtcOffset": "03:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Russian Standard Time",
  "DisplayName": "(UTC+03:00) Moscow, St. Petersburg, Volgograd",
  "StandardName": "Russia TZ 2 Standard Time",
  "DaylightName": "Russia TZ 2 Daylight Time",
  "BaseUtcOffset": "03:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "E. Africa Standard Time",
  "DisplayName": "(UTC+03:00) Nairobi",
  "StandardName": "E. Africa Standard Time",
  "DaylightName": "E. Africa Daylight Time",
  "BaseUtcOffset": "03:00:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "Iran Standard Time",
  "DisplayName": "(UTC+03:30) Tehran",
  "StandardName": "Iran Standard Time",
  "DaylightName": "Iran Daylight Time",
  "BaseUtcOffset": "03:30:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Arabian Standard Time",
  "DisplayName": "(UTC+04:00) Abu Dhabi, Muscat",
  "StandardName": "Arabian Standard Time",
  "DaylightName": "Arabian Daylight Time",
  "BaseUtcOffset": "04:00:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "Astrakhan Standard Time",
  "DisplayName": "(UTC+04:00) Astrakhan, Ulyanovsk",
  "StandardName": "Astrakhan Standard Time",
  "DaylightName": "Astrakhan Daylight Time",
  "BaseUtcOffset": "04:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Azerbaijan Standard Time",
  "DisplayName": "(UTC+04:00) Baku",
  "StandardName": "Azerbaijan Standard Time",
  "DaylightName": "Azerbaijan Daylight Time",
  "BaseUtcOffset": "04:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Russia Time Zone 3",
  "DisplayName": "(UTC+04:00) Izhevsk, Samara",
  "StandardName": "Russia TZ 3 Standard Time",
  "DaylightName": "Russia TZ 3 Daylight Time",
  "BaseUtcOffset": "04:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Mauritius Standard Time",
  "DisplayName": "(UTC+04:00) Port Louis",
  "StandardName": "Mauritius Standard Time",
  "DaylightName": "Mauritius Daylight Time",
  "BaseUtcOffset": "04:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Georgian Standard Time",
  "DisplayName": "(UTC+04:00) Tbilisi",
  "StandardName": "Georgian Standard Time",
  "DaylightName": "Georgian Daylight Time",
  "BaseUtcOffset": "04:00:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "Caucasus Standard Time",
  "DisplayName": "(UTC+04:00) Yerevan",
  "StandardName": "Caucasus Standard Time",
  "DaylightName": "Caucasus Daylight Time",
  "BaseUtcOffset": "04:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Afghanistan Standard Time",
  "DisplayName": "(UTC+04:30) Kabul",
  "StandardName": "Afghanistan Standard Time",
  "DaylightName": "Afghanistan Daylight Time",
  "BaseUtcOffset": "04:30:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "West Asia Standard Time",
  "DisplayName": "(UTC+05:00) Ashgabat, Tashkent",
  "StandardName": "West Asia Standard Time",
  "DaylightName": "West Asia Daylight Time",
  "BaseUtcOffset": "05:00:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "Ekaterinburg Standard Time",
  "DisplayName": "(UTC+05:00) Ekaterinburg",
  "StandardName": "Russia TZ 4 Standard Time",
  "DaylightName": "Russia TZ 4 Daylight Time",
  "BaseUtcOffset": "05:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Pakistan Standard Time",
  "DisplayName": "(UTC+05:00) Islamabad, Karachi",
  "StandardName": "Pakistan Standard Time",
  "DaylightName": "Pakistan Daylight Time",
  "BaseUtcOffset": "05:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "India Standard Time",
  "DisplayName": "(UTC+05:30) Chennai, Kolkata, Mumbai, New Delhi",
  "StandardName": "India Standard Time",
  "DaylightName": "India Daylight Time",
  "BaseUtcOffset": "05:30:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "Sri Lanka Standard Time",
  "DisplayName": "(UTC+05:30) Sri Jayawardenepura",
  "StandardName": "Sri Lanka Standard Time",
  "DaylightName": "Sri Lanka Daylight Time",
  "BaseUtcOffset": "05:30:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "Nepal Standard Time",
  "DisplayName": "(UTC+05:45) Kathmandu",
  "StandardName": "Nepal Standard Time",
  "DaylightName": "Nepal Daylight Time",
  "BaseUtcOffset": "05:45:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "Central Asia Standard Time",
  "DisplayName": "(UTC+06:00) Astana",
  "StandardName": "Central Asia Standard Time",
  "DaylightName": "Central Asia Daylight Time",
  "BaseUtcOffset": "06:00:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "Bangladesh Standard Time",
  "DisplayName": "(UTC+06:00) Dhaka",
  "StandardName": "Bangladesh Standard Time",
  "DaylightName": "Bangladesh Daylight Time",
  "BaseUtcOffset": "06:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "N. Central Asia Standard Time",
  "DisplayName": "(UTC+06:00) Novosibirsk",
  "StandardName": "Russia TZ 5 Standard Time",
  "DaylightName": "Russia TZ 5 Daylight Time",
  "BaseUtcOffset": "06:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Myanmar Standard Time",
  "DisplayName": "(UTC+06:30) Yangon (Rangoon)",
  "StandardName": "Myanmar Standard Time",
  "DaylightName": "Myanmar Daylight Time",
  "BaseUtcOffset": "06:30:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "SE Asia Standard Time",
  "DisplayName": "(UTC+07:00) Bangkok, Hanoi, Jakarta",
  "StandardName": "SE Asia Standard Time",
  "DaylightName": "SE Asia Daylight Time",
  "BaseUtcOffset": "07:00:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "Altai Standard Time",
  "DisplayName": "(UTC+07:00) Barnaul, Gorno-Altaysk",
  "StandardName": "Altai Standard Time",
  "DaylightName": "Altai Daylight Time",
  "BaseUtcOffset": "07:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "W. Mongolia Standard Time",
  "DisplayName": "(UTC+07:00) Hovd",
  "StandardName": "W. Mongolia Standard Time",
  "DaylightName": "W. Mongolia Daylight Time",
  "BaseUtcOffset": "07:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "North Asia Standard Time",
  "DisplayName": "(UTC+07:00) Krasnoyarsk",
  "StandardName": "Russia TZ 6 Standard Time",
  "DaylightName": "Russia TZ 6 Daylight Time",
  "BaseUtcOffset": "07:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Tomsk Standard Time",
  "DisplayName": "(UTC+07:00) Tomsk",
  "StandardName": "Tomsk Standard Time",
  "DaylightName": "Tomsk Daylight Time",
  "BaseUtcOffset": "07:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "China Standard Time",
  "DisplayName": "(UTC+08:00) Beijing, Chongqing, Hong Kong, Urumqi",
  "StandardName": "China Standard Time",
  "DaylightName": "China Daylight Time",
  "BaseUtcOffset": "08:00:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "North Asia East Standard Time",
  "DisplayName": "(UTC+08:00) Irkutsk",
  "StandardName": "Russia TZ 7 Standard Time",
  "DaylightName": "Russia TZ 7 Daylight Time",
  "BaseUtcOffset": "08:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Singapore Standard Time",
  "DisplayName": "(UTC+08:00) Kuala Lumpur, Singapore",
  "StandardName": "Malay Peninsula Standard Time",
  "DaylightName": "Malay Peninsula Daylight Time",
  "BaseUtcOffset": "08:00:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "W. Australia Standard Time",
  "DisplayName": "(UTC+08:00) Perth",
  "StandardName": "W. Australia Standard Time",
  "DaylightName": "W. Australia Daylight Time",
  "BaseUtcOffset": "08:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Taipei Standard Time",
  "DisplayName": "(UTC+08:00) Taipei",
  "StandardName": "Taipei Standard Time",
  "DaylightName": "Taipei Daylight Time",
  "BaseUtcOffset": "08:00:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "Ulaanbaatar Standard Time",
  "DisplayName": "(UTC+08:00) Ulaanbaatar",
  "StandardName": "Ulaanbaatar Standard Time",
  "DaylightName": "Ulaanbaatar Daylight Time",
  "BaseUtcOffset": "08:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "North Korea Standard Time",
  "DisplayName": "(UTC+08:30) Pyongyang",
  "StandardName": "North Korea Standard Time",
  "DaylightName": "North Korea Daylight Time",
  "BaseUtcOffset": "08:30:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Aus Central W. Standard Time",
  "DisplayName": "(UTC+08:45) Eucla",
  "StandardName": "Aus Central W. Standard Time",
  "DaylightName": "Aus Central W. Daylight Time",
  "BaseUtcOffset": "08:45:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "Transbaikal Standard Time",
  "DisplayName": "(UTC+09:00) Chita",
  "StandardName": "Transbaikal Standard Time",
  "DaylightName": "Transbaikal Daylight Time",
  "BaseUtcOffset": "09:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Tokyo Standard Time",
  "DisplayName": "(UTC+09:00) Osaka, Sapporo, Tokyo",
  "StandardName": "Tokyo Standard Time",
  "DaylightName": "Tokyo Daylight Time",
  "BaseUtcOffset": "09:00:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "Korea Standard Time",
  "DisplayName": "(UTC+09:00) Seoul",
  "StandardName": "Korea Standard Time",
  "DaylightName": "Korea Daylight Time",
  "BaseUtcOffset": "09:00:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "Yakutsk Standard Time",
  "DisplayName": "(UTC+09:00) Yakutsk",
  "StandardName": "Russia TZ 8 Standard Time",
  "DaylightName": "Russia TZ 8 Daylight Time",
  "BaseUtcOffset": "09:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Cen. Australia Standard Time",
  "DisplayName": "(UTC+09:30) Adelaide",
  "StandardName": "Cen. Australia Standard Time",
  "DaylightName": "Cen. Australia Daylight Time",
  "BaseUtcOffset": "09:30:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "AUS Central Standard Time",
  "DisplayName": "(UTC+09:30) Darwin",
  "StandardName": "AUS Central Standard Time",
  "DaylightName": "AUS Central Daylight Time",
  "BaseUtcOffset": "09:30:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "E. Australia Standard Time",
  "DisplayName": "(UTC+10:00) Brisbane",
  "StandardName": "E. Australia Standard Time",
  "DaylightName": "E. Australia Daylight Time",
  "BaseUtcOffset": "10:00:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "AUS Eastern Standard Time",
  "DisplayName": "(UTC+10:00) Canberra, Melbourne, Sydney",
  "StandardName": "AUS Eastern Standard Time",
  "DaylightName": "AUS Eastern Daylight Time",
  "BaseUtcOffset": "10:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "West Pacific Standard Time",
  "DisplayName": "(UTC+10:00) Guam, Port Moresby",
  "StandardName": "West Pacific Standard Time",
  "DaylightName": "West Pacific Daylight Time",
  "BaseUtcOffset": "10:00:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "Tasmania Standard Time",
  "DisplayName": "(UTC+10:00) Hobart",
  "StandardName": "Tasmania Standard Time",
  "DaylightName": "Tasmania Daylight Time",
  "BaseUtcOffset": "10:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Vladivostok Standard Time",
  "DisplayName": "(UTC+10:00) Vladivostok",
  "StandardName": "Russia TZ 9 Standard Time",
  "DaylightName": "Russia TZ 9 Daylight Time",
  "BaseUtcOffset": "10:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Lord Howe Standard Time",
  "DisplayName": "(UTC+10:30) Lord Howe Island",
  "StandardName": "Lord Howe Standard Time",
  "DaylightName": "Lord Howe Daylight Time",
  "BaseUtcOffset": "10:30:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Bougainville Standard Time",
  "DisplayName": "(UTC+11:00) Bougainville Island",
  "StandardName": "Bougainville Standard Time",
  "DaylightName": "Bougainville Daylight Time",
  "BaseUtcOffset": "11:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Russia Time Zone 10",
  "DisplayName": "(UTC+11:00) Chokurdakh",
  "StandardName": "Russia TZ 10 Standard Time",
  "DaylightName": "Russia TZ 10 Daylight Time",
  "BaseUtcOffset": "11:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Magadan Standard Time",
  "DisplayName": "(UTC+11:00) Magadan",
  "StandardName": "Magadan Standard Time",
  "DaylightName": "Magadan Daylight Time",
  "BaseUtcOffset": "11:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Norfolk Standard Time",
  "DisplayName": "(UTC+11:00) Norfolk Island",
  "StandardName": "Norfolk Standard Time",
  "DaylightName": "Norfolk Daylight Time",
  "BaseUtcOffset": "11:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Sakhalin Standard Time",
  "DisplayName": "(UTC+11:00) Sakhalin",
  "StandardName": "Sakhalin Standard Time",
  "DaylightName": "Sakhalin Daylight Time",
  "BaseUtcOffset": "11:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Central Pacific Standard Time",
  "DisplayName": "(UTC+11:00) Solomon Is., New Caledonia",
  "StandardName": "Central Pacific Standard Time",
  "DaylightName": "Central Pacific Daylight Time",
  "BaseUtcOffset": "11:00:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "Russia Time Zone 11",
  "DisplayName": "(UTC+12:00) Anadyr, Petropavlovsk-Kamchatsky",
  "StandardName": "Russia TZ 11 Standard Time",
  "DaylightName": "Russia TZ 11 Daylight Time",
  "BaseUtcOffset": "12:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "New Zealand Standard Time",
  "DisplayName": "(UTC+12:00) Auckland, Wellington",
  "StandardName": "New Zealand Standard Time",
  "DaylightName": "New Zealand Daylight Time",
  "BaseUtcOffset": "12:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "UTC+12",
  "DisplayName": "(UTC+12:00) Coordinated Universal Time+12",
  "StandardName": "UTC+12",
  "DaylightName": "UTC+12",
  "BaseUtcOffset": "12:00:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "Fiji Standard Time",
  "DisplayName": "(UTC+12:00) Fiji",
  "StandardName": "Fiji Standard Time",
  "DaylightName": "Fiji Daylight Time",
  "BaseUtcOffset": "12:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Kamchatka Standard Time",
  "DisplayName": "(UTC+12:00) Petropavlovsk-Kamchatsky - Old",
  "StandardName": "Kamchatka Standard Time",
  "DaylightName": "Kamchatka Daylight Time",
  "BaseUtcOffset": "12:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Chatham Islands Standard Time",
  "DisplayName": "(UTC+12:45) Chatham Islands",
  "StandardName": "Chatham Islands Standard Time",
  "DaylightName": "Chatham Islands Daylight Time",
  "BaseUtcOffset": "12:45:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Tonga Standard Time",
  "DisplayName": "(UTC+13:00) Nuku'alofa",
  "StandardName": "Tonga Standard Time",
  "DaylightName": "Tonga Daylight Time",
  "BaseUtcOffset": "13:00:00",
  "SupportsDaylightSavingTime": false
},
{
  "Id": "Samoa Standard Time",
  "DisplayName": "(UTC+13:00) Samoa",
  "StandardName": "Samoa Standard Time",
  "DaylightName": "Samoa Daylight Time",
  "BaseUtcOffset": "13:00:00",
  "SupportsDaylightSavingTime": true
},
{
  "Id": "Line Islands Standard Time",
  "DisplayName": "(UTC+14:00) Kiritimati Island",
  "StandardName": "Line Islands Standard Time",
  "DaylightName": "Line Islands Daylight Time",
  "BaseUtcOffset": "14:00:00",
  "SupportsDaylightSavingTime": false
}
]

      */