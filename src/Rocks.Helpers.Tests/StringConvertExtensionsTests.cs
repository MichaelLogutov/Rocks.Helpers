using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rocks.Helpers.Tests
{
	[TestClass]
	public class StringConvertExtensionsTests
	{
		[TestMethod]
		public void ShouldConvertToDateWithDefaultFormat ()
		{
			var res = "01.02.3000".ToDate ();

			Assert.IsNotNull (res);
			Assert.AreEqual (new DateTime (3000, 2, 1), res);
		}


		[TestMethod]
		public void ShouldConvertToDateWithCustomFormat ()
		{
			var res = "05.01.2000 14:00".ToDate ("dd.MM.yyyy HH:mm");

			Assert.IsNotNull (res);
			Assert.AreEqual (new DateTime (2000, 1, 5, 14, 0, 0), res);
		}


		[TestMethod]
		public void ShouldNotConvertToDateWithCustomFormat ()
		{
			var res = "05.01.2000 14:00:00".ToDate ("dd.MM.yyyy HH:mm");

			Assert.IsNull (res);
		}


		[TestMethod]
		public void ShouldConvertToDateTimeWithDefaultFormat ()
		{
			var res = "01.02.3000 04:05:06".ToDateTime ();

			Assert.IsNotNull (res);
			Assert.AreEqual (new DateTime (3000, 2, 1, 4, 5, 6), res);
		}


		[TestMethod]
		public void ShouldConvertToDateWithDefaultFormatUsingToDateTime ()
		{
			var res = "05.01.2000".ToDateTime ();

			Assert.IsNotNull (res);
			Assert.AreEqual (new DateTime (2000, 1, 5), res);
		}


		[TestMethod]
		public void ShouldConvertToDateTimeWithCustomFormat ()
		{
			var res = "05.01.2000 14:00".ToDateTime ("dd.MM.yyyy HH:mm");

			Assert.IsNotNull (res);
			Assert.AreEqual (new DateTime (2000, 1, 5, 14, 0, 0), res);
		}


		[TestMethod]
		public void ShouldNotConvertToDateTimeWithCustomFormat ()
		{
			var res = "05.01.2000 14:00:00".ToDateTime ("dd.MM.yyyy HH:mm");

			Assert.IsNull (res);
		}
	}
}
