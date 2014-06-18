using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;

namespace Rocks.Helpers.Tests
{
	[TestClass]
	public class HashCodeExtensions
	{
		[TestMethod]
		public void CreateHashCode_TwoEqualIntsList_ReturnsEqualHashCodes ()
		{
			// arrange
			var fixture = new Fixture ();

			var a = fixture.CreateMany<int> (10).ToList ();
			var b = a.ToList ();


			// act
			var result_a = a.CreateHashCode ();
			var result_b = b.CreateHashCode ();


			// assert
			result_a.Should ().Be (result_b);
		}


		[TestMethod]
		public void CreateHashCode_TwoNotEqualIntsList_ReturnsNotEqualHashCodes ()
		{
			// arrange
			var fixture = new Fixture ();

			var a = fixture.CreateMany<int> (10).ToList ();
			var b = fixture.CreateMany<int> (9).ToList ();


			// act
			var result_a = a.CreateHashCode ();
			var result_b = b.CreateHashCode ();


			// assert
			result_a.Should ().NotBe (result_b);
		}


		[TestMethod]
		public void CreateHashCode_TwoEqualObjectsList_ReturnsEqualHashCodes ()
		{
			// arrange
			var fixture = new Fixture ();
			fixture.Customize<TestObject> (composer => composer.With (x => x.Number, fixture.Create<int> ()));

			var a = fixture.CreateMany<TestObject> (10).ToList ();
			var b = a.ToList ();


			// act
			var result_a = a.CreateHashCode ();
			var result_b = b.CreateHashCode ();


			// assert
			result_a.Should ().Be (result_b);
		}


		[TestMethod]
		public void CreateHashCode_TwoEqualObjectsListWithNestedList_ReturnsEqualHashCodes ()
		{
			// arrange
			var fixture = new Fixture ();
			fixture.Customize<TestObject> (composer => composer.With (x => x.Number, fixture.Create<int> ()));

			var a = fixture.CreateMany<TestObject> (10).Cast<object> ().ToList ();
			a.Insert (5, fixture.CreateMany<TestObject> ());
			a.Insert (8, fixture.CreateMany<int> ());

			var b = a.ToList ();


			// act
			var result_a = a.CreateHashCode ();
			var result_b = b.CreateHashCode ();


			// assert
			result_a.Should ().Be (result_b);
		}


		private class TestObject
		{
			public int Number { get; set; }


			public override int GetHashCode ()
			{
				return this.Number;
			}
		}
	}
}