using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Rocks.Helpers
{
    public static class StringFormattingExtensions
    {
        public static string ToFormattedString (this TimeSpan time,
                                                CultureInfo culture = null,
                                                TimeSpanFormatStrings formatStrings = null)
        {
            string result;

            var format_strings = formatStrings ?? TimeSpanFormatStrings.Default;

            if (time.TotalSeconds < 1)
                result = time.Milliseconds.ToString (culture) + format_strings.MSec;
            else if (time.TotalMinutes < 1)
            {
                result = time.Seconds.ToString (culture) + format_strings.Sec + " " +
                         time.Milliseconds.ToString (culture) + format_strings.MSec;
            }
            else if (time.TotalHours < 1)
            {
                result = time.Minutes.ToString (culture) + format_strings.Min + " " +
                         time.Seconds.ToString (culture) + format_strings.Sec;
            }
            else if (time.TotalDays < 1)
            {
                result = time.Hours.ToString (culture) + format_strings.Hour + " " +
                         time.Minutes.ToString (culture) + format_strings.Min;
            }
            else
            {
                result = time.Days.ToString (culture) + format_strings.Day + " " +
                         time.Hours.ToString (culture) + format_strings.Hour + " " +
                         time.Minutes.ToString (culture) + format_strings.Min;
            }

            return result;
        }


        private static readonly Regex RegexRemoveExcessiveSpaces = new Regex (@"[\ \t]+",
                                                                              RegexOptions.Compiled |
                                                                              RegexOptions.CultureInvariant |
                                                                              RegexOptions.IgnoreCase |
                                                                              RegexOptions.IgnorePatternWhitespace |
                                                                              RegexOptions.Multiline |
                                                                              RegexOptions.Singleline |
                                                                              RegexOptions.ExplicitCapture);


        /// <summary>
        /// Remove dublicate spaces and trims the string.
        /// </summary>
        /// <param name="str">Source string (can be null).</param>
        [ContractAnnotation ("str:null => null")]
        public static string NormalizeSpaces ([CanBeNull] this string str)
        {
            if (string.IsNullOrEmpty (str))
                return str;

            return RegexRemoveExcessiveSpaces.Replace (str, " ").Trim ();
        }


        /// <summary>
        /// Return string with only first char in upper case and rest part of the string unchanged.
        /// </summary>
        /// <param name="str">Source string (can be null).</param>
        [ContractAnnotation ("str:null => null")]
        public static string CapitalizeFirstChar ([CanBeNull] this string str)
        {
            if (string.IsNullOrEmpty (str) || char.IsUpper (str[0]))
                return str;

            var res = str.Substring (0, 1).ToUpper (CultureInfo.CurrentCulture);
            if (str.Length > 1)
                res += str.Substring (1);

            return res;
        }


        /// <summary>
        /// Return string with only first char in lower case and rest part of the string unchanged.
        /// </summary>
        /// <param name="str">Source string (can be null).</param>
        [ContractAnnotation ("str:null => null")]
        public static string LowerFirstChar ([CanBeNull] this string str)
        {
            if (string.IsNullOrEmpty (str) || char.IsLower (str[0]))
                return str;

            var res = str.Substring (0, 1).ToLower (CultureInfo.CurrentCulture);
            if (str.Length > 1)
                res += str.Substring (1);

            return res;
        }


        /// <summary>
        /// Return string with first char in upper case and rest part of the string in lower case.
        /// </summary>
        /// <param name="str">Source string.</param>
        [ContractAnnotation ("str:null => null")]
        public static string Capitalize ([CanBeNull] this string str)
        {
            if (string.IsNullOrEmpty (str))
                return str;

            var res = str.Substring (0, 1).ToUpper (CultureInfo.CurrentCulture);
            if (str.Length > 1)
                res += str.Substring (1).ToLower ();

            return res;
        }


        /// <summary>
        /// Returns true if all characters in the source string are in upper case.
        /// </summary>
        /// <param name="characters">Source characters (can be null).</param>
        public static bool IsAllUpperCase ([CanBeNull] this IEnumerable<char> characters)
        {
            if (characters == null)
                return false;

            return !characters.Any (Char.IsLower);
        }


        /// <summary>
        /// If source string contains only uppercase letters then it will capitalized. Otherwise only first character will be capitalized.
        /// </summary>
        /// <param name="str">Source string (can be null).</param>
        /// <param name="byWords">Specify true to capitalize by words.</param>
        [ContractAnnotation ("str:null => null")]
        public static string CapitalizeIfNeeded ([CanBeNull] this string str, bool byWords)
        {
            if (string.IsNullOrEmpty (str))
                return str;

            if (str.IsAllUpperCase ())
                return byWords ? str.CapitalizeByWords () : str.Capitalize ();

            return str.CapitalizeFirstChar ();
        }


        /// <summary>
        /// Return capitalized string by words.
        /// </summary>
        /// <param name="str">Source string (can be null).</param>
        /// <param name="culture">Culture information used for capitalization. If null <see cref="CultureInfo.CurrentCulture"/> will be used.</param>
        [ContractAnnotation ("str:null => null")]
        public static string CapitalizeByWords ([CanBeNull] this string str, CultureInfo culture = null)
        {
            if (string.IsNullOrEmpty (str))
                return str;

            culture = culture ?? CultureInfo.CurrentCulture;

            return culture.TextInfo.ToTitleCase (str.ToLower (culture));
        }


        /// <summary>
        /// Formats file size to readable short form.
        /// </summary>
        /// <param name="sizeInBytes">Size in bytes.</param>
        public static string FormatFileSize (this long sizeInBytes)
        {
            var suffixes = new[] { " B", " KB", " MB", " GB" };

            if (sizeInBytes == 0)
                return "0" + suffixes[0];

            var bytes = Math.Abs (sizeInBytes);
            var place = Convert.ToInt32 (Math.Floor (Math.Log (bytes, 1024)));
            var num = Math.Round (bytes / Math.Pow (1024, place), 1);

            return (Math.Sign (sizeInBytes) * num).ToString (CultureInfo.InvariantCulture) + suffixes[place];
        }
    }
}