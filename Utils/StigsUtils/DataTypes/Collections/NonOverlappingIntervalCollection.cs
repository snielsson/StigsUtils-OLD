// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
using System.Diagnostics;
using StigsUtils.Extensions;
namespace StigsUtils.DataTypes.Collections;

[Obsolete("Not tested")]
public class NonOverlappingIntervalCollection<T, TData> where T : IComparable<T> {
	private Func<TData?, TData, TData> UnionFunc { get; }
	private readonly SortedList<Interval<T>, TData> _intervals = new();
	public IEnumerable<KeyValuePair<Interval<T>, TData>> Intervals => _intervals;        
	public NonOverlappingIntervalCollection(Func<TData?,TData,TData> unionFunc) {
		UnionFunc = unionFunc;
	}
	public T? StartOfFirstOrDefault => _intervals.Keys.Any() ? _intervals.Keys.First().Start : default;
	public T? EndOfLastOrDefault => _intervals.Keys.Any() ? _intervals.Keys.Last().End : default;

	public int Count => _intervals.Count;
	public NonOverlappingIntervalCollection<T, TData> Add(T start, T end, TData newData) => Add(new Interval<T>(start, end), newData);

	public TData? Get(T key) {
		var index = _intervals.Keys.BinarySearch(new Interval<T>(key,key));
		if (index < 0) index =  Math.Max(~index-1,0);
		var result = default(TData);
		while (index < _intervals.Keys.Count && _intervals.Keys[index].StartsOnOrBefore(key)) {
			if(_intervals.Keys[index].Contains(key)) result = UnionFunc(result,_intervals[_intervals.Keys[index]]);
			index++;
		}
		return result;
	}
        
	public NonOverlappingIntervalCollection<T, TData> Add(Interval<T> newInterval, TData newData) {
		if (Count == 0) {
			_intervals.Add(newInterval, newData);
			return this;
		}
            
		//Find where to start looking for overlapping intervals, assuming existing intervals are sorted and non-overlapping.
		var index = _intervals.Keys.BinarySearch(newInterval);
		if (index >= 0) {
			_intervals[_intervals.Keys[index]] = UnionFunc(_intervals[_intervals.Keys[index]],newData);
			return this;
		}
		index = Math.Max(~index - 1, 0);
			
		//Find overlapping intervals, assuming existing intervals are sorted and non-overlapping.
		List<Interval<T>> overlapping = new();
		for (var i = index; i < _intervals.Keys.Count; i++) {
			Interval<T>? current = _intervals.Keys[i];
			if (current.StartsAfterEndOf(newInterval)) break;
			if (current.Overlaps(newInterval)) overlapping.Add(current);
		}

		//Modify overlapping intervals into a sequence of non-overlapping intervals, assuming existing intervals are sorted and non-overlapping. 
		foreach (Interval<T> x in overlapping) {
			var existingInterval = x;
			TData existingData = _intervals[existingInterval];
			var unifiedData = UnionFunc(newData, existingData);
			_intervals.Remove(existingInterval);
			if (newInterval.StartsBeforeStartOf(existingInterval)) {
				_intervals.Add(new Interval<T>(newInterval.Start, existingInterval.Start), newData);
				newInterval = new Interval<T>(existingInterval.Start, newInterval.End);
			} else if (existingInterval.StartsBeforeStartOf(newInterval)) {
				_intervals.Add(new Interval<T>(existingInterval.Start, newInterval.Start), existingData);
				existingInterval = new Interval<T>(newInterval.Start, existingInterval.End);
			}
			Debug.Assert(newInterval.Start.Equals(existingInterval.Start));
                
                
			if (newInterval.End.Equals(existingInterval.End)) {
				_intervals.Add(new Interval<T>(newInterval.Start, newInterval.End), unifiedData);
				return this;
			}

			if (existingInterval.EndsBeforeEndOf(newInterval)) {
				_intervals.Add(new Interval<T>(existingInterval.Start, existingInterval.End), unifiedData);
				newInterval = new Interval<T>(existingInterval.End, newInterval.End);
			}
			else {
				_intervals.Add(new Interval<T>(newInterval.Start, newInterval.End), unifiedData);
				_intervals.Add(new Interval<T>(newInterval.End, existingInterval.End), existingData);
				return this;                        
			} 
		}
		_intervals.Add(newInterval, newData);
		return this;
	}
}