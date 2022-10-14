// Copyright © 2014-2022 Stig Schmidt Nielsson. All Rights Reserved. This file is Open Source and distributed under the MIT license - see LICENSE file.
namespace StigsUtils.Extensions;

[Obsolete("Not tested")]
public static class RandomExtensions {
	
	public static T Pick<T>(this Random @this, IList<T> list) => list[@this.Next(list.Count)];
	public static char Pick(this Random @this, string str) => str[@this.Next(str.Length)];
	
	public static T PickRandom<T>(this ICollection<T> @this, Random random) {
		var index = random.Next(0, @this.Count);
		return @this.Skip(index).First();
	}
	public static T PickRandom<T>(this ICollection<T> @this, int seed = 0) => PickRandom(@this, new Random(seed));

	public static T PickRandom<T>(this IList<T> @this, Random random) {
		var index = random.Next(0, @this.Count);
		return @this[index];
	}
	public static T PickRandom<T>(this IList<T> @this, int seed = 0) => PickRandom(@this, new Random(seed));
}