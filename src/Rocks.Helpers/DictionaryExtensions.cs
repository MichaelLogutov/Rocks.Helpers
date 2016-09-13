using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Rocks.Helpers
{
	public static class DictionaryExtensions
	{
		/// <summary>
		/// Extends IDictionary.TryGetValue with default return value.
		/// </summary>
		/// <param name="dictionary">Source dictionary.</param>
		/// <param name="key">Key.</param>
		/// <param name="defaultValue">Default value.</param>
		public static TValue GetValueOrDefault<TKey, TValue> (this IDictionary<TKey, TValue> dictionary,
		                                                      TKey key,
		                                                      TValue defaultValue = default (TValue))
		{
			TValue res;

			if (dictionary.TryGetValue (key, out res))
				return res;

			return defaultValue;
		}


		/// <summary>
		/// Extends IDictionary.TryGetValue with default return value.
		/// </summary>
		/// <param name="dictionary">Source dictionary.</param>
		/// <param name="key">Key.</param>
		/// <param name="defaultValue">Default value.</param>
		public static TValue? GetValueOrDefault<TKey, TValue> (this IDictionary<TKey, TValue> dictionary, TKey key, TValue? defaultValue = null)
			where TValue : struct
		{
			TValue res;

			if (dictionary.TryGetValue (key, out res))
				return res;

			return defaultValue;
		}


		/// <summary>
		/// Gets the value from the <paramref name="dictionary" /> and if the item with specified <paramref name="key"/> not present
		/// then calls <paramref name="callback"/> to get it's value, store it in the <paramref name="dictionary"/> and returns it.
		/// </summary>
		/// <param name="dictionary">Source dictionary.</param>
		/// <param name="key">Key.</param>
		/// <param name="callback">A callback function. Can not be null.</param>
		public static TValue GetValueOrDefault<TKey, TValue> (this IDictionary<TKey, TValue> dictionary, TKey key, [NotNull] Func<TValue> callback)
		{
			callback.RequiredNotNull ("callback");

			TValue res;

			if (!dictionary.TryGetValue (key, out res))
			{
				res = callback ();
				dictionary[key] = res;
			}

			return res;
		}
        /// <summary>
		/// Extends IReadOnlyDictionary.TryGetValue with default return value.
		/// </summary>
		/// <param name="dictionary">Source dictionary.</param>
		/// <param name="key">Key.</param>
		/// <param name="defaultValue">Default value.</param>
		public static TValue GetValueOrDefault<TKey, TValue> (this IReadOnlyDictionary<TKey, TValue> dictionary,
		                                                      TKey key,
		                                                      TValue defaultValue = default (TValue))
		{
			TValue res;

			if (dictionary.TryGetValue (key, out res))
				return res;

			return defaultValue;
		}


		/// <summary>
		/// Extends IReadOnlyDictionary.TryGetValue with default return value.
		/// </summary>
		/// <param name="dictionary">Source dictionary.</param>
		/// <param name="key">Key.</param>
		/// <param name="defaultValue">Default value.</param>
		public static TValue? GetValueOrDefault<TKey, TValue> (this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key, TValue? defaultValue = null)
			where TValue : struct
		{
			TValue res;

			if (dictionary.TryGetValue (key, out res))
				return res;

			return defaultValue;
		}
	}
}