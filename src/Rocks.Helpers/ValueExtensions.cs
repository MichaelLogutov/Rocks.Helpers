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
		private static readonly Regex ValidateRegExEntityUrl = new Regex (@"^[\w-_]+$",
		                                                                  RegexOptions.Compiled |
		                                                                  RegexOptions.CultureInvariant |
		                                                                  RegexOptions.IgnoreCase |
		                                                                  RegexOptions.IgnorePatternWhitespace |
		                                                                  RegexOptions.ExplicitCapture);


		[NotNull, ContractAnnotation ("value:null => halt")]
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


		[NotNull, ContractAnnotation ("value:null => halt")]
		public static string RequiredNotNullOrWhiteSpace (this string value, string paramName, string message = null)
		{
			if (string.IsNullOrWhiteSpace (value))
				throw new ArgumentNullException (paramName, message);

			return value;
		}


		[NotNull, ContractAnnotation ("value:null => halt")]
		public static string RequiredNotNullOrEmpty (this string value, string paramName, string message = null)
		{
			if (string.IsNullOrEmpty (value))
				throw new ArgumentNullException (paramName, message);

			return value;
		}


		[NotNull, ContractAnnotation ("value:null => halt")]
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


		[NotNull, ContractAnnotation ("values:null => halt")]
		public static ICollection<T> RequiredNotNullOrEmpty<T> (this ICollection<T> values, string paramName, string message = null)
		{
			if (values == null || values.Count == 0)
				throw new ArgumentNullException (paramName, message);

			return values;
		}


		[NotNull, ContractAnnotation ("values:null => halt")]
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
			if (value > 0)
				return value;

			if (string.IsNullOrEmpty (message))
				message = string.Format ("{0} = {1} must be greater than zero.", paramName, value);

			throw new ArgumentNullException (paramName, message);
		}


		public static T RequiredEqualsTo<T> (this T value, T value2, string paramName, string message = null)
		{
			if (Equals (value, value2))
				return value;

			if (string.IsNullOrEmpty (message))
				message = string.Format ("{0} = {1} should be equals to {2}.", paramName, value, value2);

			throw new ArgumentOutOfRangeException (paramName, message);
		}


		public static T RequiredNotEqualsTo<T> (this T value, T value2, string paramName, string message = null)
		{
			if (!Equals (value, value2))
				return value;

			if (string.IsNullOrEmpty (message))
				message = string.Format ("{0} = {1} should not be equals to {2}.", paramName, value, value2);

			throw new ArgumentOutOfRangeException (paramName, message);
		}


		[ContractAnnotation ("value:null => null")]
		public static T? RequiredNotLessThan<T> (this T? value, T minValue, string paramName, string message = null) where T : struct, IComparable
		{
			if (value != null)
				value.Value.RequiredNotLessThan (minValue, paramName, message);

			return value;
		}


		[ContractAnnotation ("value:null => null")]
		public static T? RequiredNotLessThan<T> (this T? value, T? minValue, string paramName, string message = null) where T : struct, IComparable
		{
			if (value != null && minValue != null)
				value.Value.RequiredNotLessThan (minValue.Value, paramName, message);

			return value;
		}


		public static T RequiredNotLessThan<T> (this T value, T minValue, string paramName, string message = null) where T : struct, IComparable
		{
			if (value.CompareTo (minValue) >= 0)
				return value;

			if (string.IsNullOrEmpty (message))
				message = string.Format ("{0} = {1} can not be less than {2}", paramName, value, minValue);

			throw new ArgumentOutOfRangeException (paramName, message);
		}


		[ContractAnnotation ("value:null => null")]
		public static T? RequiredNotLessOrEqualsThan<T> (this T? value, T minValue, string paramName, string message = null) where T : struct, IComparable
		{
			if (value != null)
				value.Value.RequiredNotLessOrEqualsThan (minValue, paramName, message);

			return value;
		}


		[ContractAnnotation ("value:null => null")]
		public static T? RequiredNotLessOrEqualsThan<T> (this T? value, T? minValue, string paramName, string message = null) where T : struct, IComparable
		{
			if (value != null && minValue != null)
				value.Value.RequiredNotLessOrEqualsThan (minValue.Value, paramName, message);

			return value;
		}


		public static T RequiredNotLessOrEqualsThan<T> (this T value, T minValue, string paramName, string message = null) where T : struct, IComparable
		{
			if (value.CompareTo (minValue) <= 0)
			{
				if (string.IsNullOrEmpty (message))
					message = string.Format ("{0} = {1} can not be less or equals than {2}.", paramName, value, minValue);

				throw new ArgumentOutOfRangeException (paramName, message);
			}

			return value;
		}


		[ContractAnnotation ("value:null => null")]
		public static T? RequiredNotGreaterThan<T> (this T? value, T maxValue, string paramName, string message = null) where T : struct, IComparable
		{
			if (value != null)
				value.Value.RequiredNotGreaterThan (maxValue, paramName, message);

			return value;
		}


		[ContractAnnotation ("value:null => null")]
		public static T? RequiredNotGreaterThan<T> (this T? value, T? maxValue, string paramName, string message = null) where T : struct, IComparable
		{
			if (value != null && maxValue != null)
				value.Value.RequiredNotGreaterThan (maxValue.Value, paramName, message);

			return value;
		}


		public static T RequiredNotGreaterThan<T> (this T value, T maxValue, string paramName, string message = null) where T : struct, IComparable
		{
			if (value.CompareTo (maxValue) <= 0)
				return value;

			if (string.IsNullOrEmpty (message))
				message = string.Format ("{0} = {1} can not be greater than {2}.", paramName, value, maxValue);

			throw new ArgumentOutOfRangeException (paramName, message);
		}


		[ContractAnnotation ("value:null => null")]
		public static T? RequiredNotGreaterOrEqualsThan<T> (this T? value, T maxValue, string paramName, string message = null)
			where T : struct, IComparable
		{
			if (value != null)
				value.Value.RequiredNotGreaterOrEqualsThan (maxValue, paramName, message);

			return value;
		}


		[ContractAnnotation ("value:null => null")]
		public static T? RequiredNotGreaterOrEqualsThan<T> (this T? value, T? maxValue, string paramName, string message = null)
			where T : struct, IComparable
		{
			if (value != null && maxValue != null)
				value.Value.RequiredNotGreaterOrEqualsThan (maxValue.Value, paramName, message);

			return value;
		}


		public static T RequiredNotGreaterOrEqualsThan<T> (this T value, T maxValue, string paramName, string message = null) where T : struct, IComparable
		{
			if (value.CompareTo (maxValue) < 0)
				return value;

			if (string.IsNullOrEmpty (message))
				message = string.Format ("{0} = {1} can not be greater or equals than {2}.", paramName, value, maxValue);

			throw new ArgumentOutOfRangeException (paramName, message);
		}


		[ContractAnnotation ("value:null => null")]
		public static T? RequiredIn<T> (this T? value, T minValue, T maxValue, string paramName, string message = null) where T : struct, IComparable
		{
			if (value != null)
				value.Value.RequiredIn (minValue, maxValue, paramName, message);

			return value;
		}


		[ContractAnnotation ("value:null => null")]
		public static T? RequiredIn<T> (this T? value, T? minValue, T? maxValue, string paramName, string message = null) where T : struct, IComparable
		{
			if (value == null)
				return null;

			if (minValue != null && maxValue != null)
				value.Value.RequiredIn (minValue.Value, maxValue.Value, paramName, message);
			else if (minValue != null)
				value.Value.RequiredNotLessThan (minValue.Value, paramName, message);
			else if (maxValue != null)
				value.Value.RequiredNotGreaterThan (maxValue.Value, paramName, message);

			return value;
		}


		public static T RequiredIn<T> (this T value, T minValue, T maxValue, string paramName, string message = null) where T : IComparable
		{
			if (value.CompareTo (minValue) >= 0 && value.CompareTo (maxValue) <= 0)
				return value;

			if (string.IsNullOrEmpty (message))
				message = string.Format ("{0} = {1} should be within interval {2} .. {3}.", paramName, value, minValue, maxValue);

			throw new ArgumentOutOfRangeException (paramName, message);
		}


		[ContractAnnotation ("value:null => null")]
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
			if (Enum.GetValues (typeof (T)).Cast<T> ().Contains (value))
				return value;

			if (string.IsNullOrEmpty (message))
				message = string.Format ("{0} = {1} should be valid {2} enum value.", paramName, value, typeof (T).Name);

			throw new ArgumentOutOfRangeException (paramName, message);
		}


		[ContractAnnotation ("values:null => null")]
		public static IEnumerable<T> RequiredAll<T> (this IEnumerable<T> values, Action<T> check)
		{
			if (values == null)
				return null;

			foreach (var value in values)
				check (value);

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


		[CanBeNull, ContractAnnotation ("value:null => null")]
		public static string Trimmed (this string value)
		{
			return value == null ? null : value.Trim ();
		}


		[ContractAnnotation ("value:null => null")]
		public static string AsValidEmail (this string value, string paramName, string message = null)
		{
			if (string.IsNullOrEmpty (value) || value.Count (x => x == '@') == 1)
				return value;

			if (string.IsNullOrEmpty (message))
				message = string.Format ("Parameter \"{0}\" = \"{1}\" has invalid email format.", paramName, value);

			throw new FormatException (message);
		}


		[ContractAnnotation ("value:null => null")]
		public static string AsValidEntityUrl (this string value, string paramName, string message = null)
		{
			if (string.IsNullOrEmpty (value) || ValidateRegExEntityUrl.IsMatch (value))
				return value;

			if (string.IsNullOrEmpty (message))
				message = string.Format ("Parameter \"{0}\" = \"{1}\" has invalid entity url format.", paramName, value);

			throw new FormatException (message);
		}
	}
}