using System;
using System.Collections.Generic;
using System.Linq;

namespace Rocks.Helpers
{
	public static class RandomizationExtensions
	{
		private static readonly Random _globalRandom = new Random ();

		[ThreadStatic]
		private static Random _localRandom;

		/// <summary>
		/// Gets the thread safe initialized instance of <see cref="Random"/> class.
		/// </summary>
		public static Random Random
		{
			get
			{
				var inst = _localRandom;

				if (inst == null)
				{
					int seed;

					lock (_globalRandom)
						seed = _globalRandom.Next ();

					_localRandom = inst = new Random (seed);
				}

				return inst;
			}
		}


		/// <summary>
		/// Returns next random boolean value.
		/// </summary>
		/// <param name="rand"><see cref="Random"/> object instance.</param>
		public static bool NextBool (this Random rand)
		{
			return rand.Next (100) >= 50;
		}


		/// <summary>
		/// Returns next random boolean value with given probability.
		/// </summary>
		/// <param name="rand"><see cref="Random"/> object instance.</param>
		/// <param name="trueProbability">A probability of "true" result (should be between 0.0 and 1.0).</param>
		public static bool NextBool (this Random rand, double trueProbability)
		{
			// ReSharper disable once CompareOfFloatsByEqualityOperator
			if (trueProbability == 0.0)
				return false;

			// ReSharper disable once CompareOfFloatsByEqualityOperator
			if (trueProbability == 1.0)
				return true;

			return rand.NextDouble () > (1.0 - trueProbability);
		}


		/// <summary>
		/// Returns next random item from the <paramref name="items"/> list.
		/// </summary>
		/// <typeparam name="T">Type of the item.</typeparam>
		/// <param name="rand"><see cref="Random"/> object instance.</param>
		/// <param name="items">Items list.</param>
		public static T Next<T> (this Random rand, IList<T> items)
		{
			return items[rand.Next (items.Count)];
		}


		/// <summary>
		/// Returns next random item from the <paramref name="items"/> list.
		/// </summary>
		/// <typeparam name="T">Type of the item.</typeparam>
		/// <param name="rand"><see cref="Random"/> object instance.</param>
		/// <param name="items">Items list.</param>
		public static T Next<T> (this Random rand, IEnumerable<T> items)
		{
			return items.Skip (rand.Next (items.Count ())).First ();
		}


		/// <summary>
		/// Returns random unique items from the <paramref name="items"/> list. Maximum <paramref name="count"/> items will be returned.
		/// </summary>
		/// <typeparam name="T">Type of the item.</typeparam>
		/// <param name="rand"><see cref="Random"/> object instance.</param>
		/// <param name="items">Items list.</param>
		/// <param name="count">Number of items to return.</param>
		public static IEnumerable<T> Next<T> (this Random rand, IList<T> items, int count)
		{
			var indexes = items.Select ((t, k) => k).ToList ();
			while (count-- > 0 && indexes.Count > 0)
			{
				var k = rand.Next (indexes.Count);
				var index = indexes[k];
				indexes.RemoveAt (k);

				yield return items[index];
			}
		}


		/// <summary>
		/// Returns next random enumeration value.
		/// </summary>
		/// <typeparam name="T">Type of the enum.</typeparam>
		/// <param name="rand"><see cref="Random"/> object instance.</param>
		public static T NextEnum<T> (this Random rand)
		{
			return rand.Next (Enum.GetValues (typeof (T)).Cast<T> ());
		}


		/// <summary>
		/// Returns next random double value within given range.
		/// </summary>
		/// <param name="rand"><see cref="Random"/> object instance.</param>
		/// <param name="min">Minimal value.</param>
		/// <param name="max">Maximval value/</param>
		public static double NextDouble (this Random rand, double min, double max)
		{
			if (min > max)
			{
				var tmp = min;
				min = max;
				max = tmp;
			}

			return min + rand.NextDouble () * (max - min);
		}


		/// <summary>
		/// Returns next random double value within given range.
		/// </summary>
		/// <param name="rand"><see cref="Random"/> object instance.</param>
		/// <param name="min">Minimal value.</param>
		/// <param name="max">Maximval value/</param>
		public static decimal NextDecimal (this Random rand, decimal min, decimal max)
		{
			if (min > max)
			{
				var tmp = min;
				min = max;
				max = tmp;
			}

			return min + (decimal) rand.NextDouble () * (max - min);
		}


		/// <summary>
		/// Returns random element from the <paramref name="items"/>.
		/// </summary>
		/// <typeparam name="T">Item type.</typeparam>
		/// <param name="items">Items.</param>
		public static T RandomElement<T> (this IList<T> items)
		{
			return items[Random.Next (items.Count)];
		}


		/// <summary>
		/// Returns random element from the <paramref name="items"/>.
		/// </summary>
		/// <typeparam name="T">Item type.</typeparam>
		/// <param name="items">Items.</param>
		public static T RandomElement<T> (this T[] items)
		{
			return items[Random.Next (items.Length)];
		}


		/// <summary>
		/// Returns random element from the <paramref name="items"/>.
		/// </summary>
		/// <typeparam name="T">Item type.</typeparam>
		/// <param name="items">Items.</param>
		public static int RandomIndex<T> (this ICollection<T> items)
		{
			if (items.Count == 0)
				throw new InvalidOperationException ("Cannot get random index because the collection is empty");

			return Random.Next (items.Count);
		}


		/// <summary>
		/// Returns new list contains all <paramref name="items"/> in randomized order.
		/// </summary>
		/// <typeparam name="T">Item type.</typeparam>
		/// <param name="items">Items.</param>
		public static IEnumerable<T> Randomize<T> (this IEnumerable<T> items)
		{
			return from item in items
				   orderby Random.Next ()
				   select item;
		}
	}
}
