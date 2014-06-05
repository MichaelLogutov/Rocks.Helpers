using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rocks.Helpers.Tests
{
	[TestClass]
	public class StringExtensionsTests
	{
		[TestMethod]
		public void ShouldSplitToTrimmedNonEmptyStringList ()
		{
			var res = "a.1, b, c 2".SplitToTrimmedNonEmptyStringList ();

			Assert.IsNotNull (res);
			CollectionAssert.AreEquivalent (new[] { "a.1", "b", "c 2" }, res.ToList ());
		}
	}
}