using System;
using System.Runtime.CompilerServices;

namespace Rocks.Helpers
{
    public static class RangeExtensions
    {
        /// <summary>
        ///     Restricts a <paramref name="value" /> in specific range.
        /// </summary>
        /// <typeparam name="T">Type of the values.</typeparam>
        /// <param name="value">Value.</param>
        /// <param name="min">Minimal range value.</param>
        /// <param name="max">Maximum range value.</param>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static T Clamp<T> (this T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo (max) > 0)
                return max;

            if (value.CompareTo (min) < 0)
                return min;

            return value;
        }


        /// <summary>
        ///     Restricts a <paramref name="value" /> minimal value.
        /// </summary>
        /// <typeparam name="T">Type of the values.</typeparam>
        /// <param name="value">Value.</param>
        /// <param name="min">Minimal value.</param>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static T Min<T> (this T value, T min) where T : IComparable<T>
        {
            return value.CompareTo (min) < 0 ? min : value;
        }


        /// <summary>
        ///     Restricts a <paramref name="value" /> maximum value.
        /// </summary>
        /// <typeparam name="T">Type of the values.</typeparam>
        /// <param name="value">Value.</param>
        /// <param name="max">Maximum value.</param>
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static T Max<T> (this T value, T max) where T : IComparable<T>
        {
            return value.CompareTo (max) > 0 ? max : value;
        }


        /// <summary>
        ///     Check if specified <paramref name="value" /> is withing specified range (inclusive).
        /// </summary>
        /// <typeparam name="T">Type of the values.</typeparam>
        /// <param name="value">Value.</param>
        /// <param name="min">Minimal range value (inclusive).</param>
        /// <param name="max">Maximum range value (inclusive).</param>
        public static bool In<T> (this T value, T min, T max) where T : IComparable<T>
        {
            return value.CompareTo (min) <= 0 && value.CompareTo (max) >= 0;
        }


        /// <summary>
        ///     Check if specified <paramref name="value" /> is withing specified range (inclusive).
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="min">Minimal range value (inclusive).</param>
        /// <param name="max">Maximal range value (inclusive).</param>
        public static bool In (this int value, int min, int max)
        {
            return value >= min && value <= max;
        }


        /// <summary>
        ///     Check if specified <paramref name="value" /> is withing specified range (inclusive).
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="min">Minimal range value (inclusive).</param>
        /// <param name="max">Maximal range value (inclusive).</param>
        public static bool In (this long value, long min, long max)
        {
            return value >= min && value <= max;
        }


        /// <summary>
        ///     Check if specified <paramref name="value" /> is withing specified range (inclusive).
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="min">Minimal range value (inclusive).</param>
        /// <param name="max">Maximal range value (inclusive).</param>
        public static bool In (this double value, double min, double max)
        {
            return value >= min && value <= max;
        }


        /// <summary>
        ///     Check if specified <paramref name="value" /> is withing specified range (inclusive).
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="min">Minimal range value (inclusive).</param>
        /// <param name="max">Maximal range value (inclusive).</param>
        public static bool In (this float value, float min, float max)
        {
            return value >= min && value <= max;
        }


        /// <summary>
        ///     Check if specified <paramref name="value" /> is withing specified range (inclusive).
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="min">Minimal range value (inclusive).</param>
        /// <param name="max">Maximal range value (inclusive).</param>
        public static bool In (this DateTime value, DateTime min, DateTime max)
        {
            return value >= min && value <= max;
        }


        /// <summary>
        ///     Check if specified <paramref name="value" /> is withing specified range (inclusive).
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="min">Minimal range value (inclusive).</param>
        /// <param name="max">Maximal range value (inclusive).</param>
        public static bool In (this TimeSpan value, TimeSpan min, TimeSpan max)
        {
            return value >= min && value <= max;
        }
    }
}