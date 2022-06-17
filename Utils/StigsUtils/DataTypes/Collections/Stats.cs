// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.
using StigsUtils.Extensions;
namespace StigsUtils.DataTypes.Collections;

/// <summary>
///   Stats is a collection for holding a set of decimal observations and the derived statistics of these.
/// </summary>
public class Stats { //: IEnumerable<decimal> {

	public Stats() { }

	public Stats(params decimal[] values) {
		Add(values);
	}

	public Stats(IEnumerable<decimal> values) {
		Add(values);
	}

	private readonly SortedList<decimal, int> _items = new();
	public List<SortedSet<decimal>> Modes = new() { new SortedSet<decimal>() };

	public int Count { get; private set; }
	public decimal Min { get; private set; } = decimal.MaxValue;
	public decimal Max { get; private set; } = decimal.MinValue;
	public decimal Sum { get; private set; }
	public decimal? Median {
		get {
			if (Count == 0) return null;
			if (Count == 1) return _items.First().Key;
			var midPoint = Count / 2;
			var i = 0;
			decimal? prev = null;
			if (Count.IsOdd()) {
				foreach ((var key, var val) in _items) {
					i += val;
					if (i > midPoint) return key;
					prev = key;
				}
			}
			else {
				var dist = -1;
				foreach ((var key, var val) in _items) {
					if (dist == 0) return (prev + key) / 2;
					i += val;
					dist = i - midPoint;
					if (dist >= 2) return key;
					if (dist > 0) return prev == null ? key : (prev + key) / 2;
					prev = key;
				}
			}
			return prev;
		}
	}
	public decimal Mean {
		get {
			if (Count == 0) throw new Exception("Stats is empty");
			return Sum / Count;
		}
	}
	public double StdDev => Math.Sqrt(Variance);
	public double Variance {
		get {
			if (Count == 0) throw new Exception("Stats is empty");
			if (Count == 1) return 0;
			var mean = Mean;
			decimal sum = 0;
			foreach ((var value, var count) in _items) {
				var diff = value - mean;
				sum += count * diff * diff;
			}
			return (double)sum / (Count - 1);
		}
	}
	public IEnumerable<decimal> Values => GetValues();

	public Stats Add(IEnumerable<decimal> values) {
		foreach (var value in values) AddCore(value);
		return this;
	}

	public Stats Add(params decimal[] values) {
		foreach (var value in values) AddCore(value);
		return this;
	}

	private void AddCore(decimal x) {
		_items.TryGetValue(x, out var val);
		_items[x] = ++val;
		if (val > 1) Modes[val - 1].Remove(x);
		if (Modes.Count == val) Modes.Add(new SortedSet<decimal> { x });
		else Modes[val].Add(x);

		if (x < Min) Min = x;
		if (x > Max) Max = x;
		Count++;
		Sum += x;
	}

	private IEnumerable<decimal> GetValues() {
		foreach ((var key, var val) in _items) {
			for (var i = 0; i < val; i++) yield return key;
		}
	}
	// public IEnumerator<decimal> GetEnumerator() => _values.GetEnumerator();
	// IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	public override string ToString() => $@"Stats:
Count: {Count}	
Min: {Min}
Max: {Max}
Mean: {Mean}
Median: {Median}
Modes: {string.Join(',', Modes.Last())}
";
}