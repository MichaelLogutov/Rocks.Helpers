using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Rocks.Helpers
{
	public static class StringExtensions
	{
		/// <summary>
		/// Trims the source string to specified length if it exceeds it.
		/// Optionally adds <paramref name="prefix"/> to the end of the string if it was truncated.
		/// If source string is less that <paramref name="maxLength"/> then the source string returned.
		/// </summary>
		/// <param name="str">Source string (can be null).</param>
		/// <param name="maxLength">Maximum length.</param>
		/// <param name="prefix">Optional prefix to be added to the resulting string if the source string was trimmed.</param>
		[ContractAnnotation ("str:null => null")]
		public static string TrimLength ([CanBeNull] this string str, int maxLength, string prefix = null)
		{
			if (str == null || str.Length <= maxLength)
				return str;

			var result = str.Substring (0, maxLength);
			if (prefix != null)
				result += prefix;

			return result;
		}


		/// <summary>
		/// Compares two string using <see cref="StringComparison.OrdinalIgnoreCase"/>.
		/// </summary>
		/// <param name="strA">String A (can be null).</param>
		/// <param name="strB">Stirng B (can be null).</param>
		public static bool IsEqualIgnoreCase ([CanBeNull] this string strA, [CanBeNull] string strB)
		{
			return string.Compare (strA, strB, StringComparison.OrdinalIgnoreCase) == 0;
		}


		[ContractAnnotation ("values:null => stop")]
		public static IList<string> SplitNullSafe (this string values, params char[] separator)
		{
			if (values == null)
				return new string[0];

			if (separator.IsNullOrEmpty ())
				separator = new[] { ',' };

			return values.Split (separator);
		}


		/// <summary>
		/// Splits source <paramref name="str"/> by specified <paramref name="separators"/>
		/// into a list or trimmed, non null or empty items.
		/// </summary>
		/// <param name="str">Source string (can be null).</param>
		/// <param name="separators">Separators list.</param>
		[ContractAnnotation ("str:null => stop")]
		public static IList<string> SplitToTrimmedNonEmptyStringList (this string str, params char[] separators)
		{
			return str.SplitNullSafe (separators).TrimAll ().SkipNullOrEmpty ().ToList ();
		}


		[ContractAnnotation ("values:null => stop")]
		public static IList<int> SplitToInts (this string values, params char[] separator)
		{
			return (from x in values.SplitToTrimmedNonEmptyStringList ()
			        let id = x.ToInt ()
			        where id != null
			        select id.Value).ToList ();
		}
	}
}