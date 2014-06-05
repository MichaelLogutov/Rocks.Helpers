using System;

namespace Rocks.Helpers
{
	public static class RangeExtensions
	{
		/// <summary>
		/// Restricts a <paramref name="value"/> in specific range.
		/// </summary>
		/// <typeparam name="T">Type of the values.</typeparam>
		/// <param name="value">Value.</param>
		/// <param name="min">Minimal range value.</param>
		/// <param name="max">Maximal range value.</param>
		public static T Clamp<T> (this T value, T min, T max) where T : IComparable<T>
		{
			var result = value;

			if (value.CompareTo (max) > 0)
				result = max;
			else if (value.CompareTo (min) < 0)
				result = min;

			return result;
		}


		#region In

		/// <summary>
		/// Check if specified <paramref name="value"/> is withing specified range (inclusive).
		/// </summary>
		/// <typeparam name="T">Type of the values.</typeparam>
		/// <param name="value">Value.</param>
		/// <param name="min">Minimal range value (inclusive).</param>
		/// <param name="max">Maximal range value (inclusive).</param>
		public static bool In<T> (this T value, T min, T max) where T : IComparable<T>
		{
			return value.CompareTo (min) <= 0 && value.CompareTo (max) >= 0;
		}


		/// <summary>
		/// Check if specified <paramref name="value"/> is withing specified range (inclusive).
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="min">Minimal range value (inclusive).</param>
		/// <param name="max">Maximal range value (inclusive).</param>
		public static bool In (this int value, int min, int max)
		{
			return value >= min && value <= max;
		}


		/// <summary>
		/// Check if specified <paramref name="value"/> is withing specified range (inclusive).
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="min">Minimal range value (inclusive).</param>
		/// <param name="max">Maximal range value (inclusive).</param>
		public static bool In (this long value, long min, long max)
		{
			return value >= min && value <= max;
		}


		/// <summary>
		/// Check if specified <paramref name="value"/> is withing specified range (inclusive).
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="min">Minimal range value (inclusive).</param>
		/// <param name="max">Maximal range value (inclusive).</param>
		public static bool In (this double value, double min, double max)
		{
			return value >= min && value <= max;
		}


		/// <summary>
		/// Check if specified <paramref name="value"/> is withing specified range (inclusive).
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="min">Minimal range value (inclusive).</param>
		/// <param name="max">Maximal range value (inclusive).</param>
		public static bool In (this float value, float min, float max)
		{
			return value >= min && value <= max;
		}


		/// <summary>
		/// Check if specified <paramref name="value"/> is withing specified range (inclusive).
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="min">Minimal range value (inclusive).</param>
		/// <param name="max">Maximal range value (inclusive).</param>
		public static bool In (this DateTime value, DateTime min, DateTime max)
		{
			return value >= min && value <= max;
		}


		/// <summary>
		/// Check if specified <paramref name="value"/> is withing specified range (inclusive).
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="min">Minimal range value (inclusive).</param>
		/// <param name="max">Maximal range value (inclusive).</param>
		public static bool In (this TimeSpan value, TimeSpan min, TimeSpan max)
		{
			return value >= min && value <= max;
		}

		#endregion
	}
}
