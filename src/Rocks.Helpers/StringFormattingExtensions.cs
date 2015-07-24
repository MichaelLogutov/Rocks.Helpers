using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Rocks.Helpers
{
    public static class StringFormattingExtensions
    {
        /// <summary>
        ///     Formats <see cref="TimeSpan" /> as string with specified suffixes
        ///     for each measure (days, hours, minutes, seconds and milliseconds).
        /// </summary>
        /// <param name="time">Value to be formatted.</param>
        /// <param name="formatStrings">String suffixes for formatting each measure.</param>
        /// <param name="measures">Return only specified top significant measures.</param>
        public static string ToFormattedString (this TimeSpan time,
                                                int? measures = null,
                                                TimeSpanFormatStrings formatStrings = null)
        {
            measures.RequiredNotLessOrEqualsThan (0, "measures");
            measures.RequiredNotGreaterThan (5, "measures");

            var result = new StringBuilder ();
            var format_strings = formatStrings ?? TimeSpanFormatStrings.Default;


            if (time == TimeSpan.Zero)
                return "0" + format_strings.Milliseconds;

            if (time.Ticks < 0)
            {
                result.Append ('-');
                time = time.Negate ();
            }

            
            int? first_measure = null;
            var max_measure = measures ?? 5;
            var add_separator = false;

            if (time.Days > 0)
            {
                const int current_measure = 4;

                first_measure = current_measure;

                if (first_measure - current_measure < max_measure)
                {
                    result.Append (time.Days);
                    result.Append (format_strings.Days);
                    add_separator = true;
                }
            }

            if (time.Hours > 0)
            {
                const int current_measure = 3;

                if (first_measure == null)
                    first_measure = current_measure;

                if (first_measure - current_measure < max_measure)
                {
                    if (add_separator)
                        result.Append (' ');

                    result.Append (time.Hours);
                    result.Append (format_strings.Hours);
                    add_separator = true;
                }
            }

            if (time.Minutes > 0)
            {
                const int current_measure = 2;

                if (first_measure == null)
                    first_measure = current_measure;

                if (first_measure - current_measure < max_measure)
                {
                    if (add_separator)
                        result.Append (' ');

                    result.Append (time.Minutes);
                    result.Append (format_strings.Minutes);
                    add_separator = true;
                }
            }

            if (time.Seconds > 0)
            {
                const int current_measure = 1;

                if (first_measure == null)
                    first_measure = current_measure;

                if (first_measure - current_measure < max_measure)
                {
                    if (add_separator)
                        result.Append (' ');

                    result.Append (time.Seconds);
                    result.Append (format_strings.Seconds);
                    add_separator = true;
                }
            }

            if (time.Milliseconds > 0)
            {
                const int current_measure = 0;

                if (first_measure == null)
                    first_measure = current_measure;

                if (first_measure - current_measure < max_measure)
                {
                    if (add_separator)
                        result.Append (' ');

                    result.Append (time.Milliseconds);
                    result.Append (format_strings.Milliseconds);
                    add_separator = true;
                }
            }

            return result.ToString ();
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