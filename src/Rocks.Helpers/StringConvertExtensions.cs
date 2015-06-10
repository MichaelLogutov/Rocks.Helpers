using System;
using System.Globalization;

namespace Rocks.Helpers
{
    public static class StringConvertExtensions
    {
        /// <summary>
        /// Convert string <paramref name="value"/> to bool.
        /// Returns null if value can not be parsed.
        /// </summary>
        /// <param name="value">Value as a string.</param>
        public static bool? ToBool (this string value)
        {
            bool result;
            if (!bool.TryParse (value, out result))
                return null;

            return result;
        }


        /// <summary>
        /// Convert string <paramref name="value"/> to bool.
        /// Returns <paramref name="defaultValue"/> if value can not be parsed.
        /// </summary>
        /// <param name="value">Value as a string.</param>
        /// <param name="defaultValue">Default value that will be returned if parse failed.</param>
        public static bool ToBool (this string value, bool defaultValue)
        {
            bool result;
            if (!bool.TryParse (value, out result))
                return defaultValue;

            return result;
        }


        /// <summary>
        /// Convert string <paramref name="value"/> to integer.
        /// Returns null if value can not be parsed.
        /// </summary>
        /// <param name="value">Value as a string.</param>
        /// <param name="style">A combination of styles that can be present in <paramref name="value"/> for parse to succeed.</param>
        /// <param name="culture">A culture which will be used for parsing. If null invariant culture is used.</param>
        public static int? ToInt (this string value, NumberStyles style = NumberStyles.Integer, CultureInfo culture = null)
        {
            int result;
            if (!int.TryParse (value, style, culture ?? CultureInfo.InvariantCulture, out result))
                return null;

            return result;
        }


        /// <summary>
        /// Convert string <paramref name="value"/> to integer.
        /// Returns <paramref name="defaultValue"/> if value can not be parsed.
        /// </summary>
        /// <param name="value">Value as a string.</param>
        /// <param name="defaultValue">Default value that will be returned if parse failed.</param>
        /// <param name="style">A combination of styles that can be present in <paramref name="value"/> for parse to succeed.</param>
        /// <param name="culture">A culture which will be used for parsing. If null invariant culture is used.</param>
        public static int ToInt (this string value, int defaultValue, NumberStyles style = NumberStyles.Integer, CultureInfo culture = null)
        {
            int result;
            if (!int.TryParse (value, style, culture ?? CultureInfo.InvariantCulture, out result))
                return defaultValue;

            return result;
        }


        /// <summary>
        /// Convert string <paramref name="value"/> to long.
        /// Returns null if value can not be parsed.
        /// </summary>
        /// <param name="value">Value as a string.</param>
        /// <param name="style">A combination of styles that can be present in <paramref name="value"/> for parse to succeed.</param>
        /// <param name="culture">A culture which will be used for parsing. If null invariant culture is used.</param>
        public static long? ToLong (this string value, NumberStyles style = NumberStyles.Integer, CultureInfo culture = null)
        {
            long result;
            if (!long.TryParse (value, style, culture ?? CultureInfo.InvariantCulture, out result))
                return null;

            return result;
        }


        /// <summary>
        /// Convert string <paramref name="value"/> to long.
        /// Returns <paramref name="defaultValue"/> if value can not be parsed.
        /// </summary>
        /// <param name="value">Value as a string.</param>
        /// <param name="defaultValue">Default value that will be returned if parse failed.</param>
        /// <param name="style">A combination of styles that can be present in <paramref name="value"/> for parse to succeed.</param>
        /// <param name="culture">A culture which will be used for parsing. If null invariant culture is used.</param>
        public static long ToLong (this string value, long defaultValue, NumberStyles style = NumberStyles.Integer, CultureInfo culture = null)
        {
            long result;
            if (!long.TryParse (value, style, culture ?? CultureInfo.InvariantCulture, out result))
                return defaultValue;

            return result;
        }


        /// <summary>
        /// Convert string <paramref name="value"/> to decimal.
        /// Returns null if value can not be parsed.
        /// </summary>
        /// <param name="value">Value as a string.</param>
        /// <param name="style">A combination of styles that can be present in <paramref name="value"/> for parse to succeed.</param>
        /// <param name="culture">A culture which will be used for parsing. If null invariant culture is used.</param>
        public static decimal? ToDecimal (this string value, NumberStyles style = NumberStyles.Number, CultureInfo culture = null)
        {
            decimal result;
            if (!decimal.TryParse (value, style, culture ?? CultureInfo.InvariantCulture, out result))
                return null;

            return result;
        }


        /// <summary>
        /// Convert string <paramref name="value"/> to decimal.
        /// Returns <paramref name="defaultValue"/> if value can not be parsed.
        /// </summary>
        /// <param name="value">Value as a string.</param>
        /// <param name="defaultValue">Default value that will be returned if parse failed.</param>
        /// <param name="style">A combination of styles that can be present in <paramref name="value"/> for parse to succeed.</param>
        /// <param name="culture">A culture which will be used for parsing. If null invariant culture is used.</param>
        public static decimal ToDecimal (this string value, decimal defaultValue, NumberStyles style = NumberStyles.Number, CultureInfo culture = null)
        {
            decimal result;
            if (!decimal.TryParse (value, style, culture ?? CultureInfo.InvariantCulture, out result))
                return defaultValue;

            return result;
        }


        /// <summary>
        /// Convert string <paramref name="value"/> to float.
        /// Returns null if value can not be parsed.
        /// </summary>
        /// <param name="value">Value as a string.</param>
        /// <param name="style">A combination of styles that can be present in <paramref name="value"/> for parse to succeed.</param>
        /// <param name="culture">A culture which will be used for parsing. If null invariant culture is used.</param>
        /// <param name="replaceCommaWithDot">If true, will replace all ',' characters with '.' character prior to converting.</param>
        public static float? ToFloat (this string value,
                                      NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands,
                                      CultureInfo culture = null,
                                      bool replaceCommaWithDot = true)
        {
            if (string.IsNullOrWhiteSpace (value))
                return null;

            if (replaceCommaWithDot)
                value = value.Replace (',', '.');

            float result;
            if (!float.TryParse (value, style, culture ?? CultureInfo.InvariantCulture, out result))
                return null;

            return result;
        }


        /// <summary>
        /// Convert string <paramref name="value"/> to float.
        /// Returns <paramref name="defaultValue"/> if value can not be parsed.
        /// </summary>
        /// <param name="value">Value as a string.</param>
        /// <param name="defaultValue">Default value that will be returned if parse failed.</param>
        /// <param name="style">A combination of styles that can be present in <paramref name="value"/> for parse to succeed.</param>
        /// <param name="culture">A culture which will be used for parsing. If null invariant culture is used.</param>
        /// <param name="replaceCommaWithDot">If true, will replace all ',' characters with '.' character prior to converting.</param>
        public static float ToFloat (this string value,
                                     float defaultValue,
                                     NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands,
                                     CultureInfo culture = null,
                                     bool replaceCommaWithDot = true)
        {
            if (string.IsNullOrWhiteSpace (value))
                return defaultValue;

            if (replaceCommaWithDot)
                value = value.Replace (',', '.');

            float result;
            if (!float.TryParse (value, style, culture ?? CultureInfo.InvariantCulture, out result))
                return defaultValue;

            return result;
        }


        /// <summary>
        /// Convert string <paramref name="value"/> to double.
        /// Returns null if value can not be parsed.
        /// </summary>
        /// <param name="value">Value as a string.</param>
        /// <param name="style">A combination of styles that can be present in <paramref name="value"/> for parse to succeed.</param>
        /// <param name="culture">A culture which will be used for parsing. If null invariant culture is used.</param>
        /// <param name="replaceCommaWithDot">If true, will replace all ',' characters with '.' character prior to converting.</param>
        public static double? ToDouble (this string value,
                                        NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands,
                                        CultureInfo culture = null,
                                        bool replaceCommaWithDot = true)
        {
            if (string.IsNullOrWhiteSpace (value))
                return null;

            if (replaceCommaWithDot)
                value = value.Replace (',', '.');

            double result;
            if (!double.TryParse (value, style, culture ?? CultureInfo.InvariantCulture, out result))
                return null;

            return result;
        }


        /// <summary>
        /// Convert string <paramref name="value"/> to double.
        /// Returns <paramref name="defaultValue"/> if value can not be parsed.
        /// </summary>
        /// <param name="value">Value as a string.</param>
        /// <param name="defaultValue">Default value that will be returned if parse failed.</param>
        /// <param name="style">A combination of styles that can be present in <paramref name="value"/> for parse to succeed.</param>
        /// <param name="culture">A culture which will be used for parsing. If null invariant culture is used.</param>
        /// <param name="replaceCommaWithDot">If true, will replace all ',' characters with '.' character prior to converting.</param>
        public static double ToDouble (this string value,
                                       double defaultValue,
                                       NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands,
                                       CultureInfo culture = null,
                                       bool replaceCommaWithDot = true)
        {
            if (string.IsNullOrWhiteSpace (value))
                return defaultValue;

            if (replaceCommaWithDot)
                value = value.Replace (',', '.');

            double result;
            if (!double.TryParse (value, style, culture ?? CultureInfo.InvariantCulture, out result))
                return defaultValue;

            return result;
        }


        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        public static string[] DefaultDateFormats = { "dd.MM.yyyy" };


        /// <summary>
        /// Convert string <paramref name="value"/> to date value using specified <paramref name="formats"/> and invariant culture.
        /// </summary>
        /// <param name="value">String value.</param>
        /// <param name="formats">List of datetime formats to try parse against (if null or empty the <see cref="DefaultDateFormats"/> will be used).</param>
        public static DateTime? ToDate (this string value, params string[] formats)
        {
            if (value != null)
                value = value.Trim ();
            else
                return null;

            if (string.IsNullOrEmpty (value))
                return null;

            if (formats.IsNullOrEmpty ())
                formats = DefaultDateFormats;

            return value.ToDateTime (formats);
        }


        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        public static string[] DefaultDateTimeFormats = { "dd.MM.yyyy HH:mm", "dd.MM.yyyy HH:mm:ss", "dd.MM.yyyy" };


        /// <summary>
        /// Convert string <paramref name="value"/> to datetime value using specified <paramref name="formats"/> and invariant culture.
        /// </summary>
        /// <param name="value">String value.</param>
        /// <param name="formats">List of datetime formats to try parse against (if null or empty the <see cref="DefaultDateTimeFormats"/> will be used).</param>
        public static DateTime? ToDateTime (this string value, params string[] formats)
        {
            if (value != null)
                value = value.Trim ();
            else
                return null;

            if (string.IsNullOrEmpty (value))
                return null;

            if (formats.IsNullOrEmpty ())
                formats = DefaultDateTimeFormats;

            foreach (var format in formats)
            {
                if (string.IsNullOrEmpty (format))
                    throw new InvalidOperationException ("One of the formats is null or empty string");

                DateTime res;
                if (DateTime.TryParseExact (value, formats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out res))
                    return res;
            }

            return null;
        }


        /// <summary>
        /// Convert string <paramref name="value"/> to <see cref="TimeSpan"/> value.
        /// Returns null if convertion failed.
        /// </summary>
        /// <param name="value">String value.</param>
        /// <param name="format">Format that will be used when converting <paramref name="value"/>.</param>
        /// <param name="culture">Culture that will be used when converting <paramref name="value"/>. If null the <see cref="CultureInfo.InvariantCulture"/> will be used.</param>
        public static TimeSpan? ToTime (this string value, string format = null, CultureInfo culture = null)
        {
            if (value != null)
                value = value.Trim ();
            else
                return null;

            if (string.IsNullOrEmpty (value))
                return null;

            TimeSpan res;
            if (!string.IsNullOrEmpty (format))
            {
                if (!TimeSpan.TryParseExact (value, format, culture ?? CultureInfo.InvariantCulture, out res))
                    return null;
            }
            else if (!TimeSpan.TryParse (value, culture ?? CultureInfo.InvariantCulture, out res))
                return null;

            return res;
        }


        /// <summary>
        /// Convert string <paramref name="value"/> to enum value (ignoring case) of specified type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of the enum.</typeparam>
        /// <param name="value">String value.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <param name="ignoreCase">If true then the case of the <paramref name="value"/> is ignored.</param>
        public static T ToEnum<T> (this string value, T defaultValue, bool ignoreCase = true)
        {
            if (value == null)
                return defaultValue;

            try
            {
                return (T) Enum.Parse (typeof (T), value, true);
            }
            catch (ArgumentException)
            {
                return defaultValue;
            }
        }


        /// <summary>
        /// Convert string <paramref name="value"/> to enum value (ignoring case) of specified type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of the enum.</typeparam>
        /// <param name="value">String value.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <param name="ignoreCase">If true then the case of the <paramref name="value"/> is ignored.</param>
        public static T? ToEnum<T> (this string value, T? defaultValue, bool ignoreCase = true) where T : struct
        {
            if (value == null)
                return defaultValue;

            try
            {
                return (T) Enum.Parse (typeof (T), value.Trim (), true);
            }
            catch (ArgumentException)
            {
                return defaultValue;
            }
        }
    }
}