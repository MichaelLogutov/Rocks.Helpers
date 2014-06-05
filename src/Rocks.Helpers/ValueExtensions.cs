using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Rocks.Helpers
{
	public static class ValueExtensions
	{
		private static readonly Regex ValidateRegExEmail =
			new Regex (@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
			           RegexOptions.Compiled |
			           RegexOptions.CultureInvariant |
			           RegexOptions.IgnoreCase |
			           RegexOptions.IgnorePatternWhitespace |
			           RegexOptions.ExplicitCapture);


		private static readonly Regex ValidateRegExEntityUrl = new Regex (@"^[\w-_]+$",
		                                                                  RegexOptions.Compiled |
		                                                                  RegexOptions.CultureInvariant |
		                                                                  RegexOptions.IgnoreCase |
		                                                                  RegexOptions.IgnorePatternWhitespace |
		                                                                  RegexOptions.ExplicitCapture);


		[ContractAnnotation ("value:null => halt"), NotNull]
		public static T RequiredNotNull<T> (this T value, string paramName, string message = null) where T : class
		{
			if (value == null)
				throw new ArgumentNullException (paramName, message);

			return value;
		}


		[ContractAnnotation ("value:null => halt")]
		public static T RequiredNotNull<T> (this T? value, string paramName, string message = null) where T : struct
		{
			if (value == null)
				throw new ArgumentNullException (paramName, message);

			return value.Value;
		}


		[ContractAnnotation ("value:null => halt"), NotNull]
		public static string RequiredNotNullOrWhiteSpace (this string value, string paramName, string message = null)
		{
			if (string.IsNullOrWhiteSpace (value))
				throw new ArgumentNullException (paramName, message);

			return value;
		}


		[ContractAnnotation ("value:null => halt"), NotNull]
		public static string RequiredNotNullOrEmpty (this string value, string paramName, string message = null)
		{
			if (string.IsNullOrEmpty (value))
				throw new ArgumentNullException (paramName, message);

			return value;
		}


		[ContractAnnotation ("value:null => halt"), NotNull]
		public static T RequiredNotNullOrEmpty<T> (this T value, string paramName, string message = null) where T : class
		{
			if (value == null)
				throw new ArgumentNullException (paramName, message);

			var e = value as IEnumerable;
			if (e != null)
			{
				var en = e.GetEnumerator ();
				if (!en.MoveNext ())
					throw new ArgumentNullException (paramName, message);
			}

			return value;
		}


		[ContractAnnotation ("values:null => halt"), NotNull]
		public static ICollection<T> RequiredNotNullOrEmpty<T> (this ICollection<T> values, string paramName, string message = null)
		{
			if (values == null || values.Count == 0)
				throw new ArgumentNullException (paramName, message);

			return values;
		}


		[ContractAnnotation ("values:null => halt"), NotNull]
		public static IEnumerable<T> RequiredNotNullOrEmpty<T> (this IEnumerable<T> values, string paramName, string message = null)
		{
			if (values == null || !values.Any ())
				throw new ArgumentNullException (paramName, message);

			return values;
		}


		public static DateTimeOffset Required (this DateTimeOffset value, string paramName, string message = null)
		{
			if (value == DateTimeOffset.MinValue)
				throw new ArgumentNullException (paramName, message);

			return value;
		}


		public static int RequiredId (this int value, string paramName, string message = null)
		{
			if (value <= 0)
				throw new ArgumentNullException (paramName, message ?? paramName + " must be greater than zero.");

			return value;
		}


		public static T RequiredEqualsTo<T> (this T value, T value2, string paramName)
		{
			if (!Equals (value, value2))
				throw new ArgumentOutOfRangeException (paramName, string.Format ("{0} should be equals to {1}", paramName, value2));

			return value;
		}


		public static T RequiredNotEqualsTo<T> (this T value, T value2, string paramName)
		{
			if (Equals (value, value2))
				throw new ArgumentOutOfRangeException (paramName, string.Format ("{0} should not be equals to {1}", paramName, value2));

			return value;
		}


		public static T? RequiredNotLessThan<T> (this T? value, T minValue, string paramName) where T : struct, IComparable
		{
			if (value != null)
				value.Value.RequiredNotLessThan (minValue, paramName);

			return value;
		}


		public static T? RequiredNotLessThan<T> (this T? value, T? minValue, string paramName) where T : struct, IComparable
		{
			if (value != null && minValue != null)
				value.Value.RequiredNotLessThan (minValue.Value, paramName);

			return value;
		}


		public static T RequiredNotLessThan<T> (this T value, T minValue, string paramName) where T : struct, IComparable
		{
			if (value.CompareTo (minValue) < 0)
				throw new ArgumentOutOfRangeException (paramName, string.Format ("{0} can not be less than {1}", paramName, minValue));

			return value;
		}


		public static T? RequiredNotLessOrEqualsThan<T> (this T? value, T minValue, string paramName) where T : struct, IComparable
		{
			if (value != null)
				value.Value.RequiredNotLessOrEqualsThan (minValue, paramName);

			return value;
		}


		public static T? RequiredNotLessOrEqualsThan<T> (this T? value, T? minValue, string paramName) where T : struct, IComparable
		{
			if (value != null && minValue != null)
				value.Value.RequiredNotLessOrEqualsThan (minValue.Value, paramName);

			return value;
		}


		public static T RequiredNotLessOrEqualsThan<T> (this T value, T minValue, string paramName) where T : struct, IComparable
		{
			if (value.CompareTo (minValue) <= 0)
				throw new ArgumentOutOfRangeException (paramName, string.Format ("{0} can not be less or equals than {1}", paramName, minValue));

			return value;
		}


		public static T? RequiredNotGreaterThan<T> (this T? value, T maxValue, string paramName) where T : struct, IComparable
		{
			if (value != null)
				value.Value.RequiredNotGreaterThan (maxValue, paramName);

			return value;
		}


		public static T? RequiredNotGreaterThan<T> (this T? value, T? maxValue, string paramName) where T : struct, IComparable
		{
			if (value != null && maxValue != null)
				value.Value.RequiredNotGreaterThan (maxValue.Value, paramName);

			return value;
		}


		public static T RequiredNotGreaterThan<T> (this T value, T maxValue, string paramName) where T : struct, IComparable
		{
			if (value.CompareTo (maxValue) > 0)
				throw new ArgumentOutOfRangeException (paramName, string.Format ("{0} can not be greater than {1}", paramName, maxValue));

			return value;
		}


		public static T? RequiredNotGreaterOrEqualsThan<T> (this T? value, T maxValue, string paramName) where T : struct, IComparable
		{
			if (value != null)
				value.Value.RequiredNotGreaterOrEqualsThan (maxValue, paramName);

			return value;
		}


		public static T? RequiredNotGreaterOrEqualsThan<T> (this T? value, T? maxValue, string paramName) where T : struct, IComparable
		{
			if (value != null && maxValue != null)
				value.Value.RequiredNotGreaterOrEqualsThan (maxValue.Value, paramName);

			return value;
		}


		public static T RequiredNotGreaterOrEqualsThan<T> (this T value, T maxValue, string paramName) where T : struct, IComparable
		{
			if (value.CompareTo (maxValue) >= 0)
				throw new ArgumentOutOfRangeException (paramName, string.Format ("{0} can not be greater or equals than {1}", paramName, maxValue));

			return value;
		}


		public static T? RequiredIn<T> (this T? value, T minValue, T maxValue, string paramName) where T : struct, IComparable
		{
			if (value != null)
				value.Value.RequiredIn (minValue, maxValue, paramName);

			return value;
		}


		public static T? RequiredIn<T> (this T? value, T? minValue, T? maxValue, string paramName) where T : struct, IComparable
		{
			if (value != null)
			{
				if (minValue != null && maxValue != null)
					value.Value.RequiredIn (minValue.Value, maxValue.Value, paramName);
				else if (minValue != null)
					value.Value.RequiredNotLessThan (minValue.Value, paramName);
				else if (maxValue != null)
					value.Value.RequiredNotGreaterThan (maxValue.Value, paramName);
			}

			return value;
		}


		public static T RequiredIn<T> (this T value, T minValue, T maxValue, string paramName) where T : IComparable
		{
			if (value.CompareTo (minValue) < 0 || value.CompareTo (maxValue) > 0)
				throw new ArgumentOutOfRangeException (paramName, string.Format ("{0} should be within interval {1} .. {2}", paramName, minValue, maxValue));

			return value;
		}


		public static T? RequiredEnum<T> (this T? value, string paramName, string message = null)
			where T : struct
		{
			if (value != null)
				value.Value.RequiredEnum (paramName, message);

			return value;
		}


		public static T RequiredEnum<T> (this T value, string paramName, string message = null)
			where T : struct
		{
			if (!Enum.GetValues (typeof (T)).Cast<T> ().Contains (value))
			{
				if (string.IsNullOrEmpty (message))
					message = string.Format ("{0} should be valid {1} enum value", paramName, typeof (T).Name);

				throw new ArgumentOutOfRangeException (paramName, message);
			}

			return value;
		}


		[ContractAnnotation ("values:null => null")]
		public static IEnumerable<T> RequiredAll<T> (this IEnumerable<T> values, Action<T> check)
		{
			if (values != null)
			{
				foreach (var value in values)
					check (value);
			}

			return values;
		}


		[ContractAnnotation ("value:null => defaultValue:canbenull")]
		public static string WithDefault (this string value, string defaultValue)
		{
			return string.IsNullOrWhiteSpace (value) ? defaultValue : value;
		}


		[ContractAnnotation ("value:null => defaultValue:canbenull")]
		public static T WithDefault<T> (this T value, T defaultValue) where T : class
		{
			return value ?? defaultValue;
		}


		public static T WithDefault<T> (this T? value, T defaultValue) where T : struct
		{
			return value ?? defaultValue;
		}


		[ContractAnnotation ("value:null => defaultValue:canbenull")]
		public static T WithDefaultIfNullOrEmpty<T> (this T value, T defaultValue) where T : class, IEnumerable
		{
			return value.IsNullOrEmpty () ? defaultValue : value;
		}


		public static string Trimmed (this string value)
		{
			return value == null ? null : value.Trim ();
		}


		public static string AsValidEmail (this string value, string paramName, string message = null)
		{
			if (!string.IsNullOrEmpty (value) && !ValidateRegExEmail.IsMatch (value))
				throw new FormatException (message ?? string.Format ("Parameter \"{0}\" has invalid email format: {1}", paramName, value));

			return value;
		}


		public static string AsValidEntityUrl (this string value, string paramName, string message = null)
		{
			if (!string.IsNullOrEmpty (value) && !ValidateRegExEntityUrl.IsMatch (value))
				throw new FormatException (message ?? string.Format ("Parameter \"{0}\" has invalid entity url format: {1}", paramName, value));

			return value;
		}



		[Obsolete ("This method is obsolete. Use RequiredNotNullOrEmpty or RequiredNotNullOrWhiteSpace.", true)]
		public static string Required (this string value, string paramName, string message = null)
		{
			return value.RequiredNotNullOrWhiteSpace (paramName, message);
		}


		[Obsolete ("This method is obsolete. Use RequiredNotNullOrEmpty.", true)]
		public static T Required<T> (this T value, string paramName, string message = null) where T : class
		{
			return value.RequiredNotNullOrEmpty (paramName, message);
		}


		[Obsolete ("This method is obsolete. Use RequiredNotNull.", true)]
		public static T Required<T> (this T? value, string paramName, string message = null) where T : struct
		{
			return value.RequiredNotNull (paramName, message);
		}
	}
}