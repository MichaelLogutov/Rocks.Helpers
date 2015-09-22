using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Xunit;

namespace Rocks.Helpers.Tests
{
	public class UrlExtensionsTests
	{
		[Fact]
		public void PropertiesToRouteValueDictionary_Null_ReturnsEmpty ()
		{
			var result = ((object) null).PropertiesToRouteValueDictionary ();

			result.Should ().BeEmpty ();
		}


		[Fact]
		public void PropertiesToRouteValueDictionary_NotNull_ReturnsCorrectList ()
		{
			var result = new { a = "b" }.PropertiesToRouteValueDictionary ();

			result.Should ().Equal (new Dictionary<string, object>
			{
				{ "a", "b" }
			});
		}


		private class DisplayFormatAttributeTestObject : IExplicitInterface
		{
			[DisplayFormat (DataFormatString = "{0:dd MM yyyy}")]
			public DateTime Date { get; set; }


			[DisplayFormat (DataFormatString = "{0:#.00}")]
			public decimal Number { get; set; }

			[DisplayFormat (HtmlEncode = true, NullDisplayText = "null")]
			public string HtmlEncodedString { get; set; }

			string IExplicitInterface.ExplicitProperty { get; set; }
		}


		private interface IExplicitInterface
		{
			string ExplicitProperty { get; set; }
		}


		[Fact]
		public void PropertiesToRouteValueDictionary_PropertyWithDisplayFormatAttribute_UsesDataFormatString ()
		{
			// arrange
			var obj = new DisplayFormatAttributeTestObject
			{
				Date = new DateTime (2000, 1, 2),
				Number = 1.234m
			};


			// act
			var result = obj.PropertiesToRouteValueDictionary ();


			// assert
			result.ShouldAllBeEquivalentTo (new[]
			{
				new KeyValuePair<string, object> ("Date", "02 01 2000"),
				new KeyValuePair<string, object> ("Number", "1.23"),
				new KeyValuePair<string, object> ("HtmlEncodedString", "null")
			});
		}


		[Fact]
		public void PropertiesToRouteValueDictionary_PropertyWithDisplayFormatAttribute_AndHtmlEncodeTrue_ShouldNotHtmlEncode ()
		{
			// arrange
			var obj = new DisplayFormatAttributeTestObject
			{
				HtmlEncodedString = "a b"
			};


			// act
			var result = obj.PropertiesToRouteValueDictionary ();


			// assert
			result.Should ().Contain ("HtmlEncodedString", "a b");
		}


		[Fact]
		public void PropertiesToRouteValueDictionary_ExplicitPropertySet_IgnoresExplicitProperty ()
		{
			// arrange
			var obj = new DisplayFormatAttributeTestObject ();
			((IExplicitInterface) obj).ExplicitProperty = "test";


			// act
			var result = obj.PropertiesToRouteValueDictionary ();


			// assert
			result.Should ().NotContain ("ExplicitProperty", "test");
		}


		[Fact]
		public void PropertiesToQueryParameters_Null_ReturnsEmpty ()
		{
			// arrange


			// act
			var result = ((object) null).PropertiesToQueryParameters ();


			// assert
			result.Should ().BeEmpty ();
		}


		[Fact]
		public void ToQueryStringParameters_Null_ReturnsEmpty ()
		{
			// arrange


			// act
			var result = ((IEnumerable<KeyValuePair<string, object>>) null).ToQueryStringParameters ();


			// assert
			result.Should ().BeEmpty ();
		}


		[Fact]
		public void ToQueryStringParameters_Empty_ReturnsEmpty ()
		{
			// arrange
			var data = new Dictionary<string, object> ();


			// act
			var result = data.ToQueryStringParameters ();


			// assert
			result.Should ().BeEmpty ();
		}


		[Fact]
		public void ToQueryStringParameters_OneItem_ReturnsCorrectQueryString ()
		{
			// arrange
			var data = new Dictionary<string, object>
			{
				{ "a", 1 }
			};


			// act
			var result = data.ToQueryStringParameters ();


			// assert
			result.Should ().Be ("?a=1");
		}


		[Fact]
		public void ToQueryStringParameters_TwoItems_UsesAmpersand ()
		{
			// arrange
			var data = new List<KeyValuePair<string, object>> 
			{
				new KeyValuePair<string, object> ("a", 1),
				new KeyValuePair<string, object> ("b", 2)
			};


			// act
			var result = data.ToQueryStringParameters ();


			// assert
			result.Should ().Be ("?a=1&b=2");
		}


		[Fact]
		public void ToQueryStringParameters_OneItemIsEnumerable_AddsAllItemItems ()
		{
			// arrange
			var data = new Dictionary<string, object>
			{
				{ "a", new[] { 1, 2, 3 } }
			};


			// act
			var result = data.ToQueryStringParameters ();


			// assert
			result.Should ().Be ("?a=1&a=2&a=3");
		}


		[Fact]
		public void ToQueryStringParameters_ItemNameCapitalized_LowerCaseOfTheFirstLetterForTheName ()
		{
			// arrange
			var data = new Dictionary<string, object>
			{
				{ "AaA", "BbB" }
			};


			// act
			var result = data.ToQueryStringParameters ();


			// assert
			result.Should ().Be ("?aaA=BbB");
		}


		[Fact]
		public void ToQueryStringParameters_UsesHtmlEncodeForItemValue ()
		{
			// arrange
			var data = new Dictionary<string, object>
			{
				{ "a", "a b c" }
			};


			// act
			var result = data.ToQueryStringParameters ();


			// assert
			result.Should ().Be ("?a=a%20b%20c");
		}
	}
}


