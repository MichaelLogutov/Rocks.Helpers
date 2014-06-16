using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Rocks.Helpers
{
	// ReSharper disable CompareNonConstrainedGenericWithNull

	public static class HashCodeExtensions
	{
		private const int HashCodeSeedPrimeNumber = 691;
		private const int HashCodeFieldPrimeNumber = 397;


		/// <summary>
		/// Creates hash code from given arguments.
		/// </summary>
		public static int CreateHashCode<T1, T2> (T1 obj1, T2 obj2)
		{
			var res = HashCodeSeedPrimeNumber;

			if (obj1 != null)
				res *= HashCodeFieldPrimeNumber + obj1.GetHashCode ();
			if (obj2 != null)
				res *= HashCodeFieldPrimeNumber + obj2.GetHashCode ();

			return res;
		}


		/// <summary>
		/// Creates hash code from given arguments.
		/// </summary>
		public static int CreateHashCode<T1, T2, T3> (T1 obj1, T2 obj2, T3 obj3)
		{
			var res = HashCodeSeedPrimeNumber;

			if (obj1 != null)
				res *= HashCodeFieldPrimeNumber + obj1.GetHashCode ();
			if (obj2 != null)
				res *= HashCodeFieldPrimeNumber + obj2.GetHashCode ();
			if (obj3 != null)
				res *= HashCodeFieldPrimeNumber + obj3.GetHashCode ();

			return res;
		}


		/// <summary>
		/// Creates hash code from given arguments.
		/// </summary>
		public static int CreateHashCode<T1, T2, T3, T4> (T1 obj1, T2 obj2, T3 obj3, T4 obj4)
		{
			var res = HashCodeSeedPrimeNumber;

			if (obj1 != null)
				res *= HashCodeFieldPrimeNumber + obj1.GetHashCode ();
			if (obj2 != null)
				res *= HashCodeFieldPrimeNumber + obj2.GetHashCode ();
			if (obj3 != null)
				res *= HashCodeFieldPrimeNumber + obj3.GetHashCode ();
			if (obj4 != null)
				res *= HashCodeFieldPrimeNumber + obj4.GetHashCode ();

			return res;
		}


		/// <summary>
		/// Creates "deep" hash code from list of items using recursion for nested <see cref="IEnumerable"/> objects.
		/// </summary>
		/// <param name="items">List of items. Can be null.</param>
		public static int CreateHashCode ([CanBeNull] this IEnumerable items)
		{
			if (items == null)
				return 0;

			var result = HashCodeSeedPrimeNumber;

			unchecked
			{
				foreach (var item in items)
				{
					if (item == null)
						continue;

					int item_hash_code;

					if (!(item is string))
					{
						var item_as_enumerable = item as IEnumerable;
						if (item_as_enumerable != null)
							item_hash_code = item_as_enumerable.CreateHashCode ();
						else
							item_hash_code = item.GetHashCode ();
					}
					else
						item_hash_code = item.GetHashCode ();

					result *= HashCodeFieldPrimeNumber + item_hash_code;
				}
			}

			return result;
		}


		/// <summary>
		/// Creates hash code from list of items.
		/// </summary>
		/// <param name="items">List of items. Can be null.</param>
		public static int CreateHashCode ([CanBeNull] this IEnumerable<int> items)
		{
			if (items == null)
				return 0;

			var result = HashCodeSeedPrimeNumber;

			unchecked
			{
				foreach (var item in items)
					result *= HashCodeFieldPrimeNumber + item;
			}

			return result;
		}
	}
}