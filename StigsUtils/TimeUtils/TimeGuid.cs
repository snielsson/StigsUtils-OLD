// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace StigsUtils.TimeUtils;

/// <summary>
///   A "Guid" where the first 8 bytes is a timestamp. Next 4 bytes a origin id and next 4 bytes a sequence number.
/// </summary>
public class TimeGuid {
	private static int _lastSequenceNumber;

	public TimeGuid(UtcDateTime timeStamp, int nodeId = 0, int? seq = null) {
		BitConverter.TryWriteBytes(_bytes, timeStamp.Ticks);
		BitConverter.TryWriteBytes(_bytes, nodeId);
		BitConverter.TryWriteBytes(_bytes, seq ?? Interlocked.Increment(ref _lastSequenceNumber));
	}

	private readonly byte[] _bytes = new byte[8];

	public UtcDateTime TimeStampUtc => BitConverter.ToInt64(_bytes, 0);
	public int NodeId => BitConverter.ToInt32(_bytes, 8);
	public int SequenceNumber => BitConverter.ToInt32(_bytes, 12);

	public override string ToString() => new StringBuilder(47)
		.Append(((DateTime)TimeStampUtc).ToString("O"))
		.Append('-')
		.Append(NodeId.ToString("000000000"))
		.Append('-')
		.Append(NodeId.ToString("000000000"))
		.ToString();

	public static TimeGuid Parse(ReadOnlySpan<char> x) {
		DateTime timeStamp = DateTime.Parse(x.Slice(0, 27));
		var nodeId = int.Parse(x.Slice(28, 9));
		var seq = int.Parse(x.Slice(38, 9));
		return new TimeGuid(timeStamp, nodeId, seq);
	}

	public class JsonConverter : JsonConverter<TimeGuid> {
		public override TimeGuid? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
			if (reader.TokenType is JsonTokenType.Null) return null;
			var strValue = JsonSerializer.Deserialize<string>(ref reader, options);
			return Parse(strValue);
		}

		public override void Write(Utf8JsonWriter writer, TimeGuid value, JsonSerializerOptions options) {
			throw new NotImplementedException();
		}
	}
	// public class JsonConverter : JsonConverter<TimeGuid> {
	// 	public override TimeGuid? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) { 
	// 	}
	// 	public override void Write(Utf8JsonWriter writer, TimeGuid value, JsonSerializerOptions options) {
	// 	{
	// 		if (value is null) writer.WriteNullValue();
	// 		else JsonSerializer.Serialize(writer, value.Value, options);
	// 	}	
	// }
}
// private static TimeGuid? Parse(string? strValue) {
// 	throw new NotImplementedException();
// }
// private static TimeGuid? Parse(string? strValue) {
// 	if (strValue == null) throw new ArgumentNullException(nameof(strValue));
// 		
// }
// }