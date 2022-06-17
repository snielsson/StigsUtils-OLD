// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System;
using System.Linq;
using System.Runtime.InteropServices;
using FluentAssertions;
using StigsUtils.Extensions;
using StigsUtils.TestUtils.XUnit;
using Xunit;
using Xunit.Abstractions;
namespace StigsUtils.Tests.Extensions;

public class ByteArrayExtensionsTests : XUnitTestBase {
	public ByteArrayExtensionsTests(ITestOutputHelper output) : base(output) { }
	[StructLayout(LayoutKind.Sequential)]
	public struct SomeStruct {
		public const int SizeInBytes = sizeof(long) + 2 * sizeof(int);
		public readonly long TimeStamp;
		public readonly int Bid;
		public readonly int Ask;
		public SomeStruct(long timeStamp, int bid, int ask) {
			TimeStamp = timeStamp;
			Bid = bid;
			Ask = ask;
		}
	}


	[Fact]
	public void BlitToAndFromBytesWorks() {
		var data = new[] {new SomeStruct(1, 2, 3), new SomeStruct(4, 5, 6)};
		byte[] bytes = data.BlitToBytes(SomeStruct.SizeInBytes);
		bytes.Length.Should().Be(data.Length * SomeStruct.SizeInBytes);
		var byteString = BitConverter.ToString(bytes).Dump();
		string[] byteStrings = byteString.Split('-');
		byteStrings.Length.Should().Be(bytes.Length);
		byteStrings[0].Should().Be("01");
		byteStrings[8].Should().Be("02");
		byteStrings[12].Should().Be("03");
		byteStrings[16].Should().Be("04");
		byteStrings[24].Should().Be("05");
		byteStrings[28].Should().Be("06");

		SomeStruct[] structs = bytes.BlitFromBytes<SomeStruct>(SomeStruct.SizeInBytes);
		structs.Length.Should().Be(2);
		structs[0].TimeStamp.Should().Be(1);
		structs[0].Bid.Should().Be(2);
		structs[0].Ask.Should().Be(3);
		structs[1].TimeStamp.Should().Be(4);
		structs[1].Bid.Should().Be(5);
		structs[1].Ask.Should().Be(6);

	}

	[Theory]
	[InlineData(-117)]
	[InlineData(-1)]
	[InlineData(0)]
	[InlineData(1)]
	[InlineData(117)]
	[InlineData(int.MaxValue)]
	[InlineData(int.MaxValue-1)]
	public void InsertInt(int val) {
		var target = new byte[sizeof(int)];
		target.InsertBytes(val);
		var expected = BitConverter.GetBytes(val);
		target.Should().BeEquivalentTo(expected);
		target.InsertBytes(val,0,BitConverter.IsLittleEndian);
		expected = BitConverter.GetBytes(val);
		target.Should().BeEquivalentTo(expected);
		target.InsertBytes(val,0,!BitConverter.IsLittleEndian);
		expected = BitConverter.GetBytes(val);
		target.Should().BeEquivalentTo(expected.Reverse());
	}

	[Theory]
	[InlineData(-117L)]
	[InlineData(-1L)]
	[InlineData(0L)]
	[InlineData(1L)]
	[InlineData(117L)]
	[InlineData(long.MaxValue)]
	[InlineData(long.MaxValue-1)]
	public void InsertLongWorks(long val) {
		var target = new byte[sizeof(long)];
		target.InsertBytes(val);
		var expected = BitConverter.GetBytes(val);
		target.Should().BeEquivalentTo(expected);
		target.InsertBytes(val,0,BitConverter.IsLittleEndian);
		expected = BitConverter.GetBytes(val);
		target.Should().BeEquivalentTo(expected);
		target.InsertBytes(val,0,!BitConverter.IsLittleEndian);
		expected = BitConverter.GetBytes(val);
		target.Should().BeEquivalentTo(expected.Reverse());
	}
}