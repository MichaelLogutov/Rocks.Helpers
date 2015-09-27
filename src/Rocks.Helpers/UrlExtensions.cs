using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Routing;
using FastMember;
using JetBrains.Annotations;

namespace Rocks.Helpers
{
	public static class UrlExtensions
	{
		#region Private fields

		private static readonly ConcurrentDictionary<string, TypeInfo> typesInfoCache = new ConcurrentDictionary<string, TypeInfo> (StringComparer.Ordinal);

		#endregion

		#region Static methods

		/// <summary>
		///     Creates new <see cref="RouteValueDictionary" /> from <paramref name="obj" /> properties.
		///     Optionally filters property names by <paramref name="filter" />.
		/// </summary>
		/// <param name="obj">Object which properties will be added to <see cref="RouteValueDictionary" />. Can be null.</param>
		/// <param name="filter">
		///     Optional filter function that should return true for every property and it's current value
		///     that should be included in the result.
		/// </param>
		[NotNull]
		public static RouteValueDictionary PropertiesToRouteValueDictionary ([CanBeNull] this object obj, Func<Member, object, bool> filter = null)
		{
			var res = new RouteValueDictionary ();

			if (obj == null)
				return res;

			var object_type = obj.GetType ();
			var type_accessor = TypeAccessor.Create (object_type);
			var type_info = GetTypeInfo (object_type);

			foreach (var member in type_accessor.GetMembers ())
			{
                var value = type_accessor[obj, member.Name];

				if (filter != null && !filter (member, value))
					continue;

				DisplayFormatAttribute display_format_attribute;
				if (type_info.PropertiesCustomFormat.TryGetValue (member.Name, out display_format_attribute))
					value = FormatValue (value, display_format_attribute);

				res.Add (member.Name, value);
			}

			return res;
		}


		/// <summary>
		///     Creates HTTP GET query parameters string from the supplied <paramref name="obj" /> properties.
		///     Optionally filters property names by <paramref name="filter" />.
		///     Adds specified <paramref name="prefix" /> to the start of the result string if there are any
		///     parameters in the resulting query string.
		/// </summary>
		/// <param name="obj">Object which properties will be the result query string. Can be null.</param>
		/// <param name="prefix">
		///     Prefix that will be added to the to the start of the result string if there are any
		///     parameters in the resulting query string.
		/// </param>
		/// <param name="filter">
		///     Optional filter function that should return true for every property and it's current value
		///     that should be included in the result.
		/// </param>
		public static string PropertiesToQueryParameters (this object obj, string prefix = "?", Func<Member, object, bool> filter = null)
		{
			if (obj == null)
				return string.Empty;

			return obj.PropertiesToRouteValueDictionary (filter).ToQueryStringParameters (prefix);
		}


		/// <summary>
		///     Creates HTTP GET query parameters string from the supplied <paramref name="values" />.
		///     Adds specified <paramref name="prefix" /> to the start of the result string if there are any
		///     parameters in the resulting query string.
		/// </summary>
		/// <param name="values">A list of values that will be added to the result query string.</param>
		/// <param name="prefix">
		///     Prefix that will be added to the to the start of the result string if there are any
		///     parameters in the resulting query string.
		/// </param>
		[SuppressMessage ("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
		public static string ToQueryStringParameters (this IEnumerable<KeyValuePair<string, object>> values, string prefix = "?")
		{
			if (values == null)
				return string.Empty;

			var result = new StringBuilder (250);

			foreach (var kv in values.Where (x => x.Value != null))
			{
				if (kv.Value is string)
				{
					AppendQueryStringValue (result, kv.Key, kv.Value);
					continue;
				}

				var e = kv.Value as IEnumerable;
				if (e != null)
				{
					var enm = e.GetEnumerator ();
					while (enm.MoveNext ())
						AppendQueryStringValue (result, kv.Key, enm.Current);

					continue;
				}

				AppendQueryStringValue (result, kv.Key, kv.Value);
			}

			if (result.Length == 0)
				return string.Empty;

			return prefix + result;
		}

		#endregion

		#region Private methods

		private static object FormatValue (object value, [NotNull] DisplayFormatAttribute displayFormatAttribute)
		{
			if (value == null)
				return displayFormatAttribute.NullDisplayText;

			if (string.IsNullOrEmpty (displayFormatAttribute.DataFormatString))
				return value;

			return string.Format (CultureInfo.InvariantCulture, displayFormatAttribute.DataFormatString, value);
		}


		private static TypeInfo GetTypeInfo (Type type)
		{
			TypeInfo type_info;
			var cache_key = type.FullName;

			if (typesInfoCache.TryGetValue (cache_key, out type_info))
				return type_info;

			type_info = new TypeInfo
			{
				PropertiesCustomFormat = new Dictionary<string, DisplayFormatAttribute> ()
			};


			foreach (var member in TypeAccessor.Create (type).GetMembers ())
			{
				var property = type.GetProperty (member.Name);
                if (property == null)
                    continue;

				var attr = property.GetCustomAttribute<DisplayFormatAttribute> (false);
				if (attr == null)
					continue;

				type_info.PropertiesCustomFormat[property.Name] = attr;
			}

			typesInfoCache.TryAdd (cache_key, type_info);

			return type_info;
		}


		[SuppressMessage ("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
		private static void AppendQueryStringValue ([NotNull] StringBuilder result, [NotNull] string key, [NotNull] object value, bool escapeValue = true)
		{
			var str = value is string ? (string) value : Convert.ToString (value, CultureInfo.InvariantCulture);
			if (str.Length == 0)
				return;

			if (result.Length > 0)
				result.Append ('&');

			result.Append (char.ToLower (key[0], CultureInfo.InvariantCulture));
			result.Append (key, 1, key.Length - 1);
			result.Append ('=');
			result.Append (escapeValue ? Uri.EscapeDataString (str) : str);
		}

		#endregion

		#region Nested type: TypeInfo

		private class TypeInfo
		{
			#region Public properties

			public IDictionary<string, DisplayFormatAttribute> PropertiesCustomFormat { get; set; }

			#endregion
		}

		#endregion
	}
}