using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;


// ReSharper disable ExpressionIsAlwaysNull

namespace Rocks.Helpers.Tests
{
	[TestClass]
	public class HexExtensions
	{
		[TestMethod]
		public void ToHexString_EmptyArray_ReturnsEmptyString ()
		{
			// arrange
			var data = new byte[0];


			// act
			var result = data.ToHexString ();


			// assert
			result.Should ().BeEmpty ();
		}


		[TestMethod]
		public void ToHexString_Null_ReturnsEmptyString ()
		{
			// arrange
			byte[] data = null;


			// act
			var result = data.ToHexString ();


			// assert
			result.Should ().BeEmpty ();
		}


		[TestMethod]
		public void ToHexString_ReturnsCorrectString ()
		{
			// arrange
			var data = new byte[] { 1, 10, 3, 255 };


			// act
			var result = data.ToHexString ();


			// assert
			result.Should ().Be ("010a03ff");
		}


		[TestMethod]
		public void ToHexString_UpperCase_ReturnsCorrectString ()
		{
			// arrange
			var data = new byte[] { 1, 10, 3, 255 };


			// act
			var result = data.ToHexString (true);


			// assert
			result.Should ().Be ("010A03FF");
		}


		[TestMethod]
		public void HexStringToByteArray_EmptyString_ReturnsEmptyArray ()
		{
			// arrange
			var data = string.Empty;


			// act
			var result = data.HexStringToByteArray ();


			// assert
			result.Should ().BeEmpty ();
		}


		[TestMethod]
		public void HexStringToByteArray_Null_ReturnsEmptyArray ()
		{
			// arrange
			string data = null;


			// act
			var result = data.HexStringToByteArray ();


			// assert
			result.Should ().BeEmpty ();
		}


		[TestMethod]
		public void HexStringToByteArray_ReturnsCorrectArray ()
		{
			// arrange
			var data = "010A03";


			// act
			var result = data.HexStringToByteArray ();


			// assert
			result.Should ().Equal (new byte[] { 1, 10, 3 });
		}
	}
}