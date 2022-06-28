// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
namespace StigsUtils.DataTypes;

/// <summary>
///   Datatype for half-open interval which include the start but not the end.
/// </summary>
/// <typeparam name="T"></typeparam>
public record Interval<T> : IComparable<Interval<T>>, IComparable where T : IComparable<T> {
	public Interval(T start, T end) {
		if (start.CompareTo(end) > 0) throw new Exception("start cannot be greater than end.");
		Start = start;
		End = end;
	}

	public T Start { get; init; }
	public T End { get; init; }

	public int CompareTo(object? obj) {
		if (ReferenceEquals(null, obj)) return 1;
		return obj is Interval<T> other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(Interval<T>)}");
	}

	public int CompareTo(Interval<T>? other) {
		if (other == null) throw new ArgumentNullException(nameof(other));
		var startComparison = Start.CompareTo(other.Start);
		if (startComparison != 0) return startComparison;
		return End.CompareTo(other.End);
	}

	public bool Contains(T? x) => x != null && (x.CompareTo(Start) == 0 || x.CompareTo(Start) > 0 && x.CompareTo(End) < 0);

	public bool Contains(Interval<T> x) => Contains(x.Start) && Contains(x.End);

	public bool EndsAfter(T x) => End.CompareTo(x) > 0;

	public bool EndsBeforeEndOf(Interval<T> x) => End.CompareTo(x.End) < 0;

	public bool EndsOnOrAfter(T x) => End.CompareTo(x) >= 0;

	public static Interval<T> operator +(Interval<T> x, Interval<T> y) => new(MinMax.Min(x.Start, y.Start), MinMax.Max(x.End, y.End));

	public static bool operator >(Interval<T> left, Interval<T> right) => left.CompareTo(right) > 0;

	public static bool operator >=(Interval<T> left, Interval<T> right) => left.CompareTo(right) >= 0;

	public static bool operator <(Interval<T> left, Interval<T> right) => left.CompareTo(right) < 0;

	public static bool operator <=(Interval<T> left, Interval<T> right) => left.CompareTo(right) <= 0;

	public bool Overlaps(Interval<T> x) => Contains(x.Start) || x.Contains(Start);

	public bool StartsAfter(T x) => Start.CompareTo(x) > 0;

	public bool StartsAfterEndOf(Interval<T> x) => Start.CompareTo(x.End) > 0;

	public bool StartsBeforeStartOf(Interval<T> x) => Start.CompareTo(x.Start) < 0;

	public bool StartsOnOrBefore(T x) => Start.CompareTo(x) <= 0;

	public override string ToString() => $"[{Start};{End})"; //Notation showing start is included but end is not.

	public bool TryMerge(Interval<T> x, out Interval<T> result) {
		result = this;
		if (!Overlaps(x)) return false;
		result = new Interval<T>(MinMax.Min(Start, x.Start), MinMax.Max(End, x.End));
		return true;
	}
}