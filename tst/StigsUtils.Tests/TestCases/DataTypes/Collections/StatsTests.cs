// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using StigsUtils.DataTypes.Collections;
using StigsUtils.Extensions;
using StigsUtils.TestUtils.XUnit;
using Xunit;
using Xunit.Abstractions;
namespace StigsUtils.Tests.DataTypes.Collections;

public class StatsTests : XUnitTestBase {
	public StatsTests(ITestOutputHelper output) : base(output) { }

	[Fact]
	public void StatsWorks1() {
		decimal[] items = { 3, 1, 2 };
		var sortedItems = items.OrderBy(x => x).ToArray();
		var stats = new Stats(items);

		stats.Count.Should().Be(items.Length);
		stats.Values.Should().BeEquivalentTo(sortedItems);
		stats.Mean.Should().Be(2);
		stats.Median.Should().Be(2);
		stats.Mean.Should().Be(items.Mean());

		Logger.LogInformation(stats.ToPrettyJson());

	}

	[Fact]
	public void StatsVarianceWorks() {
		var stats = new Stats(1);
		stats.Variance.Should().Be(0);

		stats = new Stats(1, 1);
		stats.Variance.Should().Be(0);

		stats = new Stats(1, 1, 1);
		stats.Variance.Should().Be(0);
		stats = new Stats(0, 1);
		stats.Variance.Should().Be(0.5);
		stats = new Stats(0, 2);
		stats.Variance.Should().Be(2);
		stats = new Stats(0, 4);
		stats.Variance.Should().Be(8);
		stats = new Stats(0, 0, 4, 4);
		stats.Variance.Should().Be(16 / (double)3);
	}

	[Fact]
	public void StatsMedianWorks() {
		var stats = new Stats(1, 2, 3);
		stats.Median.Should().Be(2);

		stats = new Stats(1, 2, 3, 4);
		stats.Median.Should().Be(new decimal(2.5));

		stats = new Stats(1, 4, 3, 2);
		stats.Median.Should().Be(new decimal(2.5));

		stats = new Stats(1);
		stats.Median.Should().Be(new decimal(1));

		stats = new Stats(1, 1);
		stats.Median.Should().Be(new decimal(1));

		stats = new Stats(1, 1, 1);
		stats.Median.Should().Be(new decimal(1));

		stats = new Stats(1, 1, 1, 1);
		stats.Median.Should().Be(new decimal(1));

		stats = new Stats(1, 1, 2, 2);
		stats.Median.Should().Be(new decimal(1.5));

		stats = new Stats(2, 2, 1, 1);
		stats.Median.Should().Be(new decimal(1.5));

		stats = new Stats(2, 2, 1, 1, 2, 2);
		stats.Median.Should().Be(new decimal(2));

		stats = new Stats(2, 2, 1, 1, 2, 2, 2);
		stats.Median.Should().Be(new decimal(2));
	}

	[Fact]
	public void StatsWorks2() {
		decimal[] items = { 4, 5, 5, 2, 2, 3 };
		var sortedItems = items.OrderBy(x => x).ToArray();
		var stats = new Stats(items);

		stats.Count.Should().Be(items.Length);
		stats.Values.Should().BeEquivalentTo(sortedItems);
		stats.Mean.Should().Be(items.Mean());
		stats.Median.Should().Be(new decimal(3.5));
		Logger.LogInformation(stats.ToPrettyJson());
		Logger.LogInformation(stats.ToString());

	}
}