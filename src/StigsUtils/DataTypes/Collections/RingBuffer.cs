// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. Code distributed under MIT license.
using System.Collections;
namespace StigsUtils.DataTypes.Collections;

public class RingBuffer<T> : IEnumerable<T> {

	public RingBuffer(int capacity) => _buffer = new T[capacity];

	private readonly T[] _buffer;
	private int _first;
	public int Count { get; private set; }
	public int Capacity => _buffer.Length;

	public T this[int i] {
		get => _buffer[GetIndex(i)];
		set => _buffer[GetIndex(i)] = value;
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	public IEnumerator<T> GetEnumerator() => new Enumerator(this);

	public RingBuffer<T> Add(T item) {
		lock (_buffer) {
			if (Count < Capacity) Count++;
			else _first++;
			if (_first == Capacity) _first = 0;
			this[Count - 1] = item;
			return this;
		}
	}

	private int GetIndex(int i) {
		if (i < 0 || i >= _buffer.Length) throw new ArgumentOutOfRangeException(nameof(i), $"Argument {nameof(i)}={i} is out of range. Valid range is [0;{_buffer.Length - 1}]");
		if (i >= _buffer.Length) throw new Exception();
		var result = _first + i;
		if (result >= _buffer.Length) result -= _buffer.Length;
		return result;
	}

	public class Enumerator : IEnumerator<T> {
		public Enumerator(RingBuffer<T> ringBuffer) {
			_ringBuffer = ringBuffer;
			i = -1;
		}

		private readonly RingBuffer<T> _ringBuffer;
		private int i;
		object? IEnumerator.Current => Current;

		public bool MoveNext() => ++i < _ringBuffer.Count;

		public void Reset() {
			i = -1;
		}

		public T Current => _ringBuffer[i];

		public void Dispose() { }
	}
}