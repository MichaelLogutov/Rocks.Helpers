using System.Collections;
using System.Collections.Generic;

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
		/// Creates hash code from list of values.
		/// </summary>
		/// <param name="values">List of values.</param>
		public static int CreateHashCode (this IEnumerable<object> values)
		{
			if (values == null)
				return 0;

			var res = HashCodeSeedPrimeNumber;

			unchecked
			{
				foreach (var v in values)
				{
					if (v == null)
						continue;

					if (!(v is string))
					{
						var ve = v as IEnumerable;
						if (ve != null)
						{
							foreach (var ve_item in ve)
								res *= HashCodeFieldPrimeNumber + new[] { ve_item }.CreateHashCode ();

							continue;
						}
					}

					res *= HashCodeFieldPrimeNumber + v.GetHashCode ();
				}
			}

			return res;
		}
	}
}