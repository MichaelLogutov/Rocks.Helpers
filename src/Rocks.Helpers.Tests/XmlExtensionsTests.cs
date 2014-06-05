using System;
using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rocks.Helpers.Tests
{
	[TestClass]
	public class XmlExtensionsTests
	{
		[TestMethod]
		public void GetElement_OneName_Exists_Returns ()
		{
			// arrange
			var root = XElement.Parse ("<root><a /></root>");


			// act
			var result = root.GetElement ("a");


			// assert
			result.Should ().Be (root.Elements ().First ());
		}


		[TestMethod]
		public void GetElement_TwoNames_Exists_Returns ()
		{
			// arrange
			var root = XElement.Parse ("<root><a><b /></a></root>");


			// act
			var result = root.GetElement ("a", "b");


			// assert
			result.Should ().Be (root.Elements ().First ().Elements ().First ());
		}


		[TestMethod]
		public void GetElement_OneName_NotExists_Creates ()
		{
			// arrange
			var root = XElement.Parse ("<root />");


			// act
			var result = root.GetElement ("a");


			// assert
			result.Name.LocalName.Should ().Be ("a");
			root.Elements ().First ().Should ().Be (result);
			root.ToString (SaveOptions.DisableFormatting).Should ().Be ("<root><a /></root>");
		}


		[TestMethod]
		public void GetElement_TwoNames_NotExists_Creates ()
		{
			// arrange
			var root = XElement.Parse ("<root />");


			// act
			var result = root.GetElement ("a", "b");


			// assert
			result.Name.LocalName.Should ().Be ("b");
			root.Elements ().First ().Elements ().First ().Should ().Be (result);
			root.ToString (SaveOptions.DisableFormatting).Should ().Be ("<root><a><b /></a></root>");
		}


		[TestMethod]
		public void GetValue_NullElement_NoName_ReturnsNull ()
		{
			// arrange


			// act
			var result = ((XElement) null).GetValue ();


			// assert
			result.Should ().BeNull ();
		}


		[TestMethod]
		public void GetValue_Exists_NoName_ReturnsValue ()
		{
			// arrange
			var root = XElement.Parse ("<root>a</root>");

			// act
			var result = root.GetValue ();


			// assert
			result.Should ().Be ("a");
		}


		[TestMethod]
		public void GetValue_NullElement_WithName_ReturnsNull ()
		{
			// arrange


			// act
			var result = ((XElement) null).GetValue ("a");


			// assert
			result.Should ().BeNull ();
		}


		[TestMethod]
		public void GetValue_Exists_WithName_ReturnsValue ()
		{
			// arrange
			var root = XElement.Parse ("<root><a>b</a></root>");

			// act
			var result = root.GetValue ("a");


			// assert
			result.Should ().Be ("b");
		}
	}
}