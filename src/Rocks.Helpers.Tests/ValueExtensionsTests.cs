using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rocks.Helpers.Tests
{
	[TestClass]
	public class ValueExtensionsTests
	{
		public enum TestEnum
		{
			A = 1,
			B = 2
		}


		[TestMethod]
		public void ShouldRequireEnum_Ok ()
		{
			var target = TestEnum.A;

			target.RequiredEnum ("target");
		}


		[TestMethod]
		public void ShouldRequireEnum_OkNullable ()
		{
			var target = (TestEnum?) TestEnum.A;

			target.RequiredEnum ("target");
		}


		[TestMethod]
		[ExpectedException (typeof (ArgumentOutOfRangeException))]
		public void ShouldRequireEnum_FailWhenInvalid ()
		{
			var target = (TestEnum) 0;

			target.RequiredEnum ("target");
		}


		[TestMethod]
		public void ShouldRequiredAll_Ok ()
		{
			var target = new[] { TestEnum.A, TestEnum.B };

			target.RequiredAll (x => x.RequiredEnum ("target"));
		}


		[TestMethod]
		[ExpectedException (typeof (ArgumentOutOfRangeException))]
		public void ShouldRequiredAll_FailWhenInvalid ()
		{
			var target = new[] { TestEnum.A, (TestEnum) 0 };

			target.RequiredAll (x => x.RequiredEnum ("target"));
		}


		[TestMethod]
		public void AsValidEntityUrl_ValidUrl_DoesNotThrows ()
		{
			// arrange
			var url = "aaa";


			// act
			var action = new Action (() => url.AsValidEntityUrl ("url"));


			// assert
			action.ShouldNotThrow ();
		}


		[TestMethod]
		public void AsValidEntityUrl_ValidUrlWithRussianChars_DoesNotThrows ()
		{
			// arrange
			var url = "абв";


			// act
			var action = new Action (() => url.AsValidEntityUrl ("url"));


			// assert
			action.ShouldNotThrow ();
		}


		[TestMethod]
		public void AsValidEntityUrl_Null_DoesNotThrows ()
		{
			// arrange
			string url = null;


			// act
			// ReSharper disable once ExpressionIsAlwaysNull
			var action = new Action (() => url.AsValidEntityUrl ("url"));


			// assert
			action.ShouldNotThrow ();
		}


		[TestMethod]
		public void AsValidEntityUrl_InvalidUrl_Throws ()
		{
			// arrange
			var url = "+aaa";


			// act
			var action = new Action (() => url.AsValidEntityUrl ("url"));


			// assert
			action.ShouldThrow<FormatException> ();
		}


		[TestMethod]
		public void AsValidEmail_ValidEmail_DoesNotThrows ()
		{
			// arrange
			var email = "a@a";


			// act
			var action = new Action (() => email.AsValidEmail ("email"));


			// assert
			action.ShouldNotThrow ();
		}


		[TestMethod]
		public void AsValidEmail_InvalidEmail_Throws ()
		{
			// arrange
			var email = "aaa";


			// act
			var action = new Action (() => email.AsValidEmail ("email"));


			// assert
			action.ShouldThrow<FormatException> ();
		}
	}
}