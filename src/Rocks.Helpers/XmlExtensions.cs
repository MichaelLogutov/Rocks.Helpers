using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using JetBrains.Annotations;

namespace Rocks.Helpers
{
	public static class XmlExtensions
	{
		/// <summary>
		///     Get the element with specified name.
		///     If element is not exist it will be created.
		/// </summary>
		/// <param name="e">Root element.</param>
		/// <param name="name">Element name.</param>
		[NotNull]
		public static XElement GetElement ([NotNull] this XElement e, [NotNull] string name)
		{
			if (e == null)
				throw new ArgumentNullException ("e");

			if (string.IsNullOrEmpty (name))
				throw new ArgumentNullException ("name");

			var n = e.Element (name);
			if (n == null)
			{
				n = new XElement (name);
				e.Add (n);
			}

			return n;
		}


		/// <summary>
		///     Get the element with specified path (as list of <paramref name="names" />).
		///     If element is not exist it will be created.
		/// </summary>
		/// <param name="e">Root element.</param>
		/// <param name="names">A path of the required element as a list of element's names.</param>
		[NotNull]
		public static XElement GetElement ([NotNull] this XElement e, [NotNull] params string[] names)
		{
			if (e == null)
				throw new ArgumentNullException ("e");

			if (names == null)
				throw new ArgumentNullException ("names");

			return e.GetElement ((IEnumerable<string>) names);
		}


		/// <summary>
		///     Get the element with specified path (as list of <paramref name="names" />).
		///     If element is not exist it will be created.
		/// </summary>
		/// <param name="e">Root element.</param>
		/// <param name="names">A path of the required element as a list of element's names.</param>
		[NotNull]
		public static XElement GetElement ([NotNull] this XElement e, [NotNull] IEnumerable<string> names)
		{
			if (e == null)
				throw new ArgumentNullException ("e");

			if (names == null)
				throw new ArgumentNullException ("names");

			foreach (var name in names)
			{
				var n = e.Element (name);
				if (n == null)
				{
					n = new XElement (name);
					e.Add (n);
				}

				e = n;
			}

			return e;
		}


		/// <summary>
		///     Removes XML elements node by specified path (if it exists).
		/// </summary>
		/// <param name="e">Root element.</param>
		/// <param name="xpath">XPath to element.</param>
		public static void RemoveElement ([NotNull] this XNode e, [NotNull] string xpath)
		{
			if (e == null)
				throw new ArgumentNullException ("e");

			if (string.IsNullOrEmpty (xpath))
				throw new ArgumentNullException ("xpath");

			var node = e.XPathSelectElement (xpath);
			if (node != null)
				node.Remove ();
		}


		/// <summary>
		///     Adds specified <paramref name="xml" /> to the source element if <paramref name="xml" /> is not null or empty.
		///     Returns source element.
		/// </summary>
		/// <param name="e">Source element.</param>
		/// <param name="xml">XML (can be null).</param>
		[NotNull]
		public static XElement AddXml ([NotNull] this XElement e, [CanBeNull] string xml)
		{
			if (e == null)
				throw new ArgumentNullException ("e");

			if (!string.IsNullOrEmpty (xml))
				e.Add (XElement.Parse (xml));

			return e;
		}


		/// <summary>
		///     Removes all elements that matched specified <paramref name="xpath" />.
		/// </summary>
		/// <param name="e">Source element.</param>
		/// <param name="xpath">XPath of the element(s) to be removed.</param>
		[NotNull]
		public static XElement Remove ([NotNull] this XElement e, [NotNull] string xpath)
		{
			if (e == null)
				throw new ArgumentNullException ("e");

			if (string.IsNullOrEmpty (xpath))
				throw new ArgumentNullException ("xpath");

			foreach (var node in e.XPathSelectElements (xpath).ToArray ())
				node.Remove ();

			return e;
		}


		/// <summary>
		///     Creates a copy of XML element with new name.
		/// </summary>
		/// <param name="e">Source element.</param>
		/// <param name="newName">New name.</param>
		[NotNull]
		public static XElement Rename ([NotNull] this XElement e, [NotNull] string newName)
		{
			if (e == null)
				throw new ArgumentNullException ("e");

			if (string.IsNullOrEmpty (newName))
				throw new ArgumentNullException ("newName");

			return new XElement (newName, e.Attributes (), e.Elements ());
		}


		/// <summary>
		///     Returns XML elements's text by specified path or null if no such element present.
		/// </summary>
		/// <param name="e">Root element.</param>
		/// <param name="xpath">XPath to element (can be null).</param>
		[CanBeNull]
		public static string GetValue ([CanBeNull] this XElement e, [CanBeNull] string xpath = null)
		{
			if (e == null)
				return null;

			if (xpath == null)
				return e.Value;

			var node = e.XPathSelectElement (xpath);
			if (node != null)
				return node.Value;

			return null;
		}


		/// <summary>
		///     Gets XML element attribute text. Returns null if no such attribute present.
		/// </summary>
		/// <param name="e">Source xml element.</param>
		/// <param name="attributeName">Name of the attribute.</param>
		[CanBeNull]
		public static string GetAttribute ([NotNull] this XElement e, [NotNull] string attributeName)
		{
			if (e == null)
				throw new ArgumentNullException ("e");

			if (string.IsNullOrEmpty (attributeName))
				throw new ArgumentNullException ("attributeName");

			var a = e.Attribute (attributeName);
			if (a != null)
				return a.Value;

			return null;
		}


		/// <summary>
		///     Sets the value of the element with name <paramref name="name" /> to <paramref name="value" />.
		///     If element is not present it will be created.
		///     If <paramref name="value" /> is null the element will be removed (if it exist).
		/// </summary>
		/// <param name="e">Root element.</param>
		/// <param name="name">Element name.</param>
		/// <param name="value">Element value.</param>
		/// <param name="culture">
		///     Culture to be used when converting <paramref name="value" /> to string. If null the
		///     <see cref="CultureInfo.InvariantCulture" /> will be used.
		/// </param>
		[NotNull]
		public static XElement SetValue ([NotNull] this XElement e, [NotNull] string name, [CanBeNull] object value, CultureInfo culture = null)
		{
			if (e == null)
				throw new ArgumentNullException ("e");

			if (string.IsNullOrEmpty (name))
				throw new ArgumentNullException ("name");

			var n = e.Element (name);
			if (n == null)
			{
				if (value != null)
					e.Add (new XElement (name, Convert.ToString (value, culture ?? CultureInfo.InvariantCulture)));
			}
			else
			{
				var str = value != null ? Convert.ToString (value, culture ?? CultureInfo.InvariantCulture) : null;

				if (!string.IsNullOrEmpty (str))
					n.Value = str;
				else
					n.Remove ();
			}

			return e;
		}


		/// <summary>
		///     Sets XML element attribute text.
		///     If attribute is not present it will be created.
		///     If <paramref name="value" /> is null or empty the attribute will be removed (if it exist).
		/// </summary>
		/// <param name="e">Source xml element.</param>
		/// <param name="attributeName">Name of the attribute.</param>
		/// <param name="value">New value.</param>
		/// <param name="culture">
		///     Culture to be used when converting <paramref name="value" /> to string. If null the
		///     <see cref="CultureInfo.InvariantCulture" /> will be used.
		/// </param>
		[NotNull]
		public static XElement SetAttribute ([NotNull] this XElement e, [NotNull] string attributeName, [CanBeNull] object value, CultureInfo culture = null)
		{
			if (e == null)
				throw new ArgumentNullException ("e");

			if (string.IsNullOrEmpty (attributeName))
				throw new ArgumentNullException ("attributeName");

			var a = e.Attribute (attributeName);
			if (a == null)
			{
				if (value != null)
					e.Add (new XAttribute (attributeName, value));
			}
			else
			{
				var str = value != null ? Convert.ToString (value, culture ?? CultureInfo.InvariantCulture) : null;

				if (!string.IsNullOrEmpty (str))
					a.Value = str;
				else
					a.Remove ();
			}

			return e;
		}

		#region ToXElement

		/// <summary>
		///     Generates new <see cref="XElement" /> with specified <paramref name="name" /> if <paramref name="value" /> is not
		///     null.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="name">Name of the <see cref="XElement" />.</param>
		/// <param name="format">Format that will be used when converting <paramref name="value" /> to string.</param>
		/// <param name="culture">
		///     Culture that will be used when converting <paramref name="value" /> to string. If null the
		///     <see cref="CultureInfo.InvariantCulture" /> will be used.
		/// </param>
		[CanBeNull, ContractAnnotation ("value:null => null")]
		public static XElement ToXElement (this byte? value, string name, string format = null, CultureInfo culture = null)
		{
			return value != null ? value.Value.ToXElement (name, format, culture) : null;
		}


		/// <summary>
		///     Generates new <see cref="XElement" /> with specified <paramref name="name" />.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="name">Name of the <see cref="XElement" />.</param>
		/// <param name="format">Format that will be used when converting <paramref name="value" /> to string.</param>
		/// <param name="culture">
		///     Culture that will be used when converting <paramref name="value" /> to string. If null the
		///     <see cref="CultureInfo.InvariantCulture" /> will be used.
		/// </param>
		[NotNull]
		public static XElement ToXElement (this byte value, string name, string format = null, CultureInfo culture = null)
		{
			return new XElement (name, value.ToString (format, culture ?? CultureInfo.InvariantCulture));
		}


		/// <summary>
		///     Generates new <see cref="XElement" /> with specified <paramref name="name" /> if <paramref name="value" /> is not
		///     null.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="name">Name of the <see cref="XElement" />.</param>
		/// <param name="format">Format that will be used when converting <paramref name="value" /> to string.</param>
		/// <param name="culture">
		///     Culture that will be used when converting <paramref name="value" /> to string. If null the
		///     <see cref="CultureInfo.InvariantCulture" /> will be used.
		/// </param>
		[CanBeNull, ContractAnnotation ("value:null => null")]
		public static XElement ToXElement (this short? value, string name, string format = null, CultureInfo culture = null)
		{
			return value != null ? value.Value.ToXElement (name, format, culture) : null;
		}


		/// <summary>
		///     Generates new <see cref="XElement" /> with specified <paramref name="name" />.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="name">Name of the <see cref="XElement" />.</param>
		/// <param name="format">Format that will be used when converting <paramref name="value" /> to string.</param>
		/// <param name="culture">
		///     Culture that will be used when converting <paramref name="value" /> to string. If null the
		///     <see cref="CultureInfo.InvariantCulture" /> will be used.
		/// </param>
		[NotNull]
		public static XElement ToXElement (this short value, string name, string format = null, CultureInfo culture = null)
		{
			return new XElement (name, value.ToString (format, culture ?? CultureInfo.InvariantCulture));
		}


		/// <summary>
		///     Generates new <see cref="XElement" /> with specified <paramref name="name" /> if <paramref name="value" /> is not
		///     null.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="name">Name of the <see cref="XElement" />.</param>
		/// <param name="format">Format that will be used when converting <paramref name="value" /> to string.</param>
		/// <param name="culture">
		///     Culture that will be used when converting <paramref name="value" /> to string. If null the
		///     <see cref="CultureInfo.InvariantCulture" /> will be used.
		/// </param>
		[CanBeNull, ContractAnnotation ("value:null => null")]
		public static XElement ToXElement (this int? value, string name, string format = null, CultureInfo culture = null)
		{
			return value != null ? value.Value.ToXElement (name, format, culture) : null;
		}


		/// <summary>
		///     Generates new <see cref="XElement" /> with specified <paramref name="name" />.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="name">Name of the <see cref="XElement" />.</param>
		/// <param name="format">Format that will be used when converting <paramref name="value" /> to string.</param>
		/// <param name="culture">
		///     Culture that will be used when converting <paramref name="value" /> to string. If null the
		///     <see cref="CultureInfo.InvariantCulture" /> will be used.
		/// </param>
		[NotNull]
		public static XElement ToXElement (this int value, string name, string format = null, CultureInfo culture = null)
		{
			return new XElement (name, value.ToString (format, culture ?? CultureInfo.InvariantCulture));
		}


		/// <summary>
		///     Generates new <see cref="XElement" /> with specified <paramref name="name" /> if <paramref name="value" /> is not
		///     null.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="name">Name of the <see cref="XElement" />.</param>
		/// <param name="format">Format that will be used when converting <paramref name="value" /> to string.</param>
		/// <param name="culture">
		///     Culture that will be used when converting <paramref name="value" /> to string. If null the
		///     <see cref="CultureInfo.InvariantCulture" /> will be used.
		/// </param>
		[CanBeNull, ContractAnnotation ("value:null => null")]
		public static XElement ToXElement (this long? value, string name, string format = null, CultureInfo culture = null)
		{
			return value != null ? value.Value.ToXElement (name, format, culture) : null;
		}


		/// <summary>
		///     Generates new <see cref="XElement" /> with specified <paramref name="name" />.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="name">Name of the <see cref="XElement" />.</param>
		/// <param name="format">Format that will be used when converting <paramref name="value" /> to string.</param>
		/// <param name="culture">
		///     Culture that will be used when converting <paramref name="value" /> to string. If null the
		///     <see cref="CultureInfo.InvariantCulture" /> will be used.
		/// </param>
		[NotNull]
		public static XElement ToXElement (this long value, string name, string format = null, CultureInfo culture = null)
		{
			return new XElement (name, value.ToString (format, culture ?? CultureInfo.InvariantCulture));
		}


		/// <summary>
		///     Generates new <see cref="XElement" /> with specified <paramref name="name" /> if <paramref name="value" /> is not
		///     null.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="name">Name of the <see cref="XElement" />.</param>
		/// <param name="format">Format that will be used when converting <paramref name="value" /> to string.</param>
		/// <param name="culture">
		///     Culture that will be used when converting <paramref name="value" /> to string. If null the
		///     <see cref="CultureInfo.InvariantCulture" /> will be used.
		/// </param>
		[CanBeNull, ContractAnnotation ("value:null => null")]
		public static XElement ToXElement (this float? value, string name, string format = null, CultureInfo culture = null)
		{
			return value != null ? value.Value.ToXElement (name, format, culture) : null;
		}


		/// <summary>
		///     Generates new <see cref="XElement" /> with specified <paramref name="name" />.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="name">Name of the <see cref="XElement" />.</param>
		/// <param name="format">Format that will be used when converting <paramref name="value" /> to string.</param>
		/// <param name="culture">
		///     Culture that will be used when converting <paramref name="value" /> to string. If null the
		///     <see cref="CultureInfo.InvariantCulture" /> will be used.
		/// </param>
		[NotNull]
		public static XElement ToXElement (this float value, string name, string format = null, CultureInfo culture = null)
		{
			return new XElement (name, value.ToString (format, culture ?? CultureInfo.InvariantCulture));
		}


		/// <summary>
		///     Generates new <see cref="XElement" /> with specified <paramref name="name" /> if <paramref name="value" /> is not
		///     null.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="name">Name of the <see cref="XElement" />.</param>
		/// <param name="format">Format that will be used when converting <paramref name="value" /> to string.</param>
		/// <param name="culture">
		///     Culture that will be used when converting <paramref name="value" /> to string. If null the
		///     <see cref="CultureInfo.InvariantCulture" /> will be used.
		/// </param>
		[CanBeNull, ContractAnnotation ("value:null => null")]
		public static XElement ToXElement (this double? value, string name, string format = null, CultureInfo culture = null)
		{
			return value != null ? value.Value.ToXElement (name, format, culture) : null;
		}


		/// <summary>
		///     Generates new <see cref="XElement" /> with specified <paramref name="name" />.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="name">Name of the <see cref="XElement" />.</param>
		/// <param name="format">Format that will be used when converting <paramref name="value" /> to string.</param>
		/// <param name="culture">
		///     Culture that will be used when converting <paramref name="value" /> to string. If null the
		///     <see cref="CultureInfo.InvariantCulture" /> will be used.
		/// </param>
		[NotNull]
		public static XElement ToXElement (this double value, string name, string format = null, CultureInfo culture = null)
		{
			return new XElement (name, value.ToString (format, culture ?? CultureInfo.InvariantCulture));
		}


		/// <summary>
		///     Generates new <see cref="XElement" /> with specified <paramref name="name" /> if <paramref name="value" /> is not
		///     null.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="name">Name of the <see cref="XElement" />.</param>
		/// <param name="format">Format that will be used when converting <paramref name="value" /> to string.</param>
		/// <param name="culture">
		///     Culture that will be used when converting <paramref name="value" /> to string. If null the
		///     <see cref="CultureInfo.InvariantCulture" /> will be used.
		/// </param>
		[CanBeNull, ContractAnnotation ("value:null => null")]
		public static XElement ToXElement (this decimal? value, string name, string format = "G29", CultureInfo culture = null)
		{
			return value != null ? value.Value.ToXElement (name, format, culture) : null;
		}


		/// <summary>
		///     Generates new <see cref="XElement" /> with specified <paramref name="name" />.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="name">Name of the <see cref="XElement" />.</param>
		/// <param name="format">Format that will be used when converting <paramref name="value" /> to string.</param>
		/// <param name="culture">
		///     Culture that will be used when converting <paramref name="value" /> to string. If null the
		///     <see cref="CultureInfo.InvariantCulture" /> will be used.
		/// </param>
		[NotNull]
		public static XElement ToXElement (this decimal value, string name, string format = "G29", CultureInfo culture = null)
		{
			return new XElement (name, value.ToString (format, culture ?? CultureInfo.InvariantCulture));
		}


		/// <summary>
		///     Generates new <see cref="XElement" /> with specified <paramref name="name" /> if <paramref name="value" /> is not
		///     null.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="name">Name of the <see cref="XElement" />.</param>
		/// <param name="format">Format that will be used when converting <paramref name="value" /> to string.</param>
		/// <param name="culture">
		///     Culture that will be used when converting <paramref name="value" /> to string. If null the
		///     <see cref="CultureInfo.InvariantCulture" /> will be used.
		/// </param>
		[CanBeNull, ContractAnnotation ("value:null => null")]
		public static XElement ToXElement (this TimeSpan? value, string name, string format = null, CultureInfo culture = null)
		{
			return value != null ? value.Value.ToXElement (name, format, culture) : null;
		}


		/// <summary>
		///     Generates new <see cref="XElement" /> with specified <paramref name="name" />.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="name">Name of the <see cref="XElement" />.</param>
		/// <param name="format">Format that will be used when converting <paramref name="value" /> to string.</param>
		/// <param name="culture">
		///     Culture that will be used when converting <paramref name="value" /> to string. If null the
		///     <see cref="CultureInfo.InvariantCulture" /> will be used.
		/// </param>
		[NotNull]
		public static XElement ToXElement (this TimeSpan value, string name, string format = null, CultureInfo culture = null)
		{
			return new XElement (name, value.ToString (format, culture ?? CultureInfo.InvariantCulture));
		}


		/// <summary>
		///     Generates new <see cref="XElement" /> with specified <paramref name="name" /> if <paramref name="value" /> is not
		///     null.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="name">Name of the <see cref="XElement" />.</param>
		/// <param name="format">Format that will be used when converting <paramref name="value" /> to string.</param>
		/// <param name="culture">
		///     Culture that will be used when converting <paramref name="value" /> to string. If null the
		///     <see cref="CultureInfo.InvariantCulture" /> will be used.
		/// </param>
		[CanBeNull, ContractAnnotation ("value:null => null")]
		public static XElement ToXElement (this DateTime? value, string name, string format = null, CultureInfo culture = null)
		{
			return value != null ? value.Value.ToXElement (name, format, culture) : null;
		}


		/// <summary>
		///     Generates new <see cref="XElement" /> with specified <paramref name="name" />.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="name">Name of the <see cref="XElement" />.</param>
		/// <param name="format">Format that will be used when converting <paramref name="value" /> to string.</param>
		/// <param name="culture">
		///     Culture that will be used when converting <paramref name="value" /> to string. If null the
		///     <see cref="CultureInfo.InvariantCulture" /> will be used.
		/// </param>
		[NotNull]
		public static XElement ToXElement (this DateTime value, string name, string format = null, CultureInfo culture = null)
		{
			return new XElement (name, value.ToString (format, culture ?? CultureInfo.InvariantCulture));
		}


		/// <summary>
		///     Generates new <see cref="XElement" /> with specified <paramref name="name" /> if <paramref name="value" /> is not
		///     null or empty.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="name">Name of the <see cref="XElement" />.</param>
		[CanBeNull, ContractAnnotation ("value:null => null")]
		public static XElement ToXElement (this string value, string name)
		{
			return !string.IsNullOrEmpty (value) ? new XElement (name, value) : null;
		}


		/// <summary>
		///     Generates new <see cref="XElement" /> with specified <paramref name="name" /> if <paramref name="value" /> is not
		///     null.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="name">Name of the <see cref="XElement" />.</param>
		/// <param name="culture">
		///     Culture that will be used when converting <paramref name="value" /> to string. If null the
		///     <see cref="CultureInfo.InvariantCulture" /> will be used.
		/// </param>
		[CanBeNull, ContractAnnotation ("value:null => null")]
		public static XElement ToXElement (this object value, string name, CultureInfo culture = null)
		{
			return value != null ? new XElement (name, Convert.ToString (value, culture ?? CultureInfo.InvariantCulture)) : null;
		}

		#endregion

		/// <summary>
		///     Loads new <see cref="XElement" /> object instance from <paramref name="assembly" /> resource (should be XML file).
		/// </summary>
		/// <param name="assembly">Assembly with the resource.</param>
		/// <param name="resourceName">Resource name.</param>
		[NotNull]
		public static XElement LoadFromResource ([NotNull] Assembly assembly, [NotNull] string resourceName)
		{
			if (assembly == null)
				throw new ArgumentNullException ("assembly");

			if (string.IsNullOrEmpty (resourceName))
				throw new ArgumentNullException ("resourceName");

			var stream = assembly.GetManifestResourceStream (resourceName);
			if (stream == null)
				throw new InvalidOperationException ("Resource '" + resourceName + "' not found in assmebly " + assembly.FullName);

			using (var xml_stream = new StreamReader (stream))
				return XElement.Load (xml_stream);
		}


		/// <summary>
		///     Converts XML to string with formatted output.
		/// </summary>
		/// <param name="xml">Source xml.</param>
		/// <param name="format">True if result needs to be formatted.</param>
		[NotNull]
		public static string ToString ([NotNull] this XNode xml, bool format)
		{
			if (xml == null)
				throw new ArgumentNullException ("xml");

			return xml.ToString (format, null);
		}


		/// <summary>
		///     Converts XML to string with formatted output.
		/// </summary>
		/// <param name="xml">Source xml.</param>
		/// <param name="format">True if result needs to be formatted.</param>
		/// <param name="encoding">Result string encoding. If null UTF-16 will be used.</param>
		[NotNull]
		public static string ToString ([NotNull] this XNode xml, bool format, Encoding encoding)
		{
			if (xml == null)
				throw new ArgumentNullException ("xml");

			encoding = encoding ?? Encoding.Unicode;

			var settings = new XmlWriterSettings { Encoding = encoding };

			if (format)
			{
				settings.Indent = true;
				settings.IndentChars = "\t";
			}

			var stream = new EncodeSpecifiedStringWriter (encoding);

			using (var xw = XmlWriter.Create (stream, settings))
			{
				xml.WriteTo (xw);
				xw.Flush ();

				return stream.ToString ();
			}
		}


		/// <summary>
		///     Converts XML to string.
		/// </summary>
		/// <param name="xml">Source xml.</param>
		/// <param name="format">True if result needs to be formatted.</param>
		[NotNull]
		public static string ToString ([NotNull] this XDocument xml, bool format)
		{
			if (xml == null)
				throw new ArgumentNullException ("xml");

			return xml.ToString (format, null);
		}


		/// <summary>
		///     Converts XML to string.
		/// </summary>
		/// <param name="xml">Source xml.</param>
		/// <param name="format">True if result needs to be formatted.</param>
		/// <param name="encoding">Result string encoding. If null UTF-16 will be used.</param>
		[NotNull]
		public static string ToString ([NotNull] this XDocument xml, bool format, Encoding encoding)
		{
			if (xml == null)
				throw new ArgumentNullException ("xml");

			using (var stream = new EncodeSpecifiedStringWriter (encoding ?? Encoding.Unicode))
			{
				xml.Save (stream, format ? SaveOptions.None : SaveOptions.DisableFormatting);

				return stream.ToString ();
			}
		}


		/// <summary>
		///     Returns new dettached element if source element is null.
		/// </summary>
		/// <param name="e">Source element.</param>
		/// <param name="name">Newly created element name.</param>
		[NotNull]
		public static XElement NewIfNull (this XElement e, [NotNull] string name)
		{
			if (e != null)
				return e;

			if (string.IsNullOrEmpty (name))
				throw new ArgumentNullException ("name");

			return new XElement (name);
		}


		/// <summary>
		///     Returns new dettached attribute if source attribute is null.
		/// </summary>
		/// <param name="attribute">Source attribute.</param>
		/// <param name="name">Newly created attribute name.</param>
		/// <param name="value">Newly created attribute value.</param>
		[NotNull]
		public static XAttribute NewIfNull (this XAttribute attribute, [NotNull] string name, [NotNull] object value)
		{
			if (attribute != null)
				return attribute;

			if (string.IsNullOrEmpty (name))
				throw new ArgumentNullException ("name");

			if (value == null)
				throw new ArgumentNullException ("value");

			return new XAttribute (name, value);
		}
	}
}