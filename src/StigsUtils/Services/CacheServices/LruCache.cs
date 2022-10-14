// // Copyright © 2021 Stig Schmidt Nielsson. This file is open source and distributed under the MIT License, see LICENSE file.
//
// using StigsUtils.Extensions;
//
// namespace StigsUtils.Common.Services.CacheServices {
// 	/// <summary>
// 	/// LruCache (Least Recently Used Cache) is a passive, self organizing data structure for caching data.
// 	/// By passive is meant that no timers or other threads are used to maintain the cache.
// 	/// The cache is self organizing in the sense that it has a max capacity, and when this capacity is reached,
// 	/// room for new a data item is created by removing the least recently used item. The least recently used
// 	/// item is the item which have not been accessed for the longest time. Items in the cache has an optional
// 	///	An item is considered accessed when it has been updated or read when not expired. If an item is expired
// 	/// when read then it is removed and the read attempt fails. The optional expiry time of an items has nothing
// 	/// to do with the "least recently used" book keeping evaluation, and there is no automatic cleaning or purging
// 	/// expired. Data is only removed when capacity is exceeded or when an expired items is attempted read. 
// 	/// </summary>
// 	public class LruCache<TKey, TVal> {
// 		public interface IItem {
// 			TKey? Key { get; }
// 			TVal? Val { get; }
// 			DateTime ExpiryTime { get; } 
// 		}
// 		private class Item : IItem {
// 			public TKey? Key { get; set; }
// 			public TVal? Val { get; set; }
// 			public DateTime ExpiryTime { get; set; }
// 			public Item? Next { get; private set; }
// 			public Item? Prev { get; private set; }
// 			public Item InsertBefore(Item? other) {
// 				Prev = other?.Prev;
// 				Next = other;
// 				if (other != null) other.Prev = this;
// 				return this;
// 			}
// 			public void Remove() {
// 				if (Prev != null) Prev.Next = Next;
// 				if (Next != null) Next.Prev = Prev;
// 				Prev = null;
// 				Next = null;
// 			}
// 		}
// 		private Item? First { get; set; }
// 		private Item? Last { get; set; }
//
// 		private readonly Dictionary<TKey, Item> _items = new();
// 		private readonly Func<DateTime> _utcTimeProvider;
// 		private long _capacity;
// 		public LruCache(long capacity, Func<DateTime> utcTimeProvider) {
// 			_utcTimeProvider = utcTimeProvider;
// 			Capacity = capacity;
// 		}
// 		public long Capacity {
// 			get => _capacity;
// 			set {
// 				_capacity = value;
// 				TrimTo(_capacity);
// 			}
// 		}
// 		public DateTime UtcNow => _utcTimeProvider().Assert(x => x.Kind == DateTimeKind.Utc, x => $"Expected DateTime to have kind Utc but got {x.Kind}.");
// 		public LruCache<TKey, TVal> Set(TKey key, TVal val, TimeSpan maxAge) => Set(new (TKey,TVal)[]{(key, val)}, UtcNow+maxAge);
// 		public LruCache<TKey, TVal> Set(IEnumerable<(TKey key, TVal val)> args, TimeSpan maxAge) => Set(args, UtcNow + maxAge);		
// 		public LruCache<TKey, TVal> Set((TKey key, TVal val)[] args, TimeSpan maxAge) => Set(args, UtcNow+maxAge);
// 		public LruCache<TKey, TVal> Set(TKey key, TVal val, DateTime? expiryTime = null) => Set(new (TKey,TVal)[]{(key, val)}, expiryTime ?? DateTime.MaxValue);
// 		public LruCache<TKey, TVal> Set(IEnumerable<(TKey key, TVal val)> args, DateTime? expiryTime = null) => Set(args.ToArray(), expiryTime ?? DateTime.MaxValue);
// 		public LruCache<TKey, TVal> Set((TKey key, TVal val)[] args, DateTime? expiryTime = null){
// 			if (args.Length == 0) return this;
// 			DateTime now = UtcNow;
// 			expiryTime ??= DateTime.MaxValue;
// 			if (now >= expiryTime) throw new InvalidOperationException($"Cannot set expired items in LruCache, expiryTime = {expiryTime}");
// 			lock (_items) {
// 				TrimTo_(Capacity - args.Length);
// 				foreach ((TKey key, TVal val) in args) {
// 					if (!_items.TryGetValue(key, out var item)) item = new Item {Key = key};
// 					item.ExpiryTime = now;
// 					item.Val = val;
// 					First = item.InsertBefore(First);
// 				}
// 				return this;
// 			}
// 		}
//
// 		
// 		public LruCache<TKey, TVal> TrimTo(long trimToCount) {
// 			lock (_items) return TrimTo_(trimToCount);
// 		}
// 		private LruCache<TKey, TVal> TrimTo_(long trimToCount) {
// 			while (_items.Count > trimToCount) {
// 				_items.Remove(Last!.Key);
// 				Item? tmp = Last.Prev;
// 				Last.Remove();
// 				Last = tmp;
// 			}
// 			return this;
// 		}
//
// 		public bool TryGet(TKey key, out TVal? val, TimeSpan? maxAge = null) {
// 			lock (_items) {
// 				if (TryGetCacheItem_(key, out var cacheItem, maxAge)) {
// 					val = cacheItem.Val;
// 					return true;
// 				}
// 				val = default;
// 				return false;
// 			}
// 		}
// 		public bool TryGetCacheItem(TKey key, out IItem? item, TimeSpan? maxAge = null) {
// 			lock (_items) return TryGetCacheItem_(key, out item, maxAge);
// 		}
// 		private bool TryGetCacheItem_(TKey key, out IItem? item, TimeSpan? maxAge = null) {
// 			if (_items.TryGetValue(key, out var node)) {
// 				if (maxAge == null || UtcNow - node.ExpiryTime <= maxAge) {
// 					item = First = node.InsertBefore(First);
// 					return true;
// 				}
// 			}
// 			item = default;
// 			return false;
// 		}
//
// 	}
// }