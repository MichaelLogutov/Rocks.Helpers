using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rocks.Helpers.Tests
{
	[TestClass]
	public class StringConvertExtensionsTests
	{
		[TestMethod]
		public void ToDate_DefaultFormat_Converts ()
		{
			var res = "01.02.3000".ToDate ();

			Assert.IsNotNull (res);
			Assert.AreEqual (new DateTime (3000, 2, 1), res);
		}


		[TestMethod]
		public void ToDate_CustomFormat_Converts ()
		{
			var res = "05.01.2000 14:00".ToDate ("dd.MM.yyyy HH:mm");

			Assert.IsNotNull (res);
			Assert.AreEqual (new DateTime (2000, 1, 5, 14, 0, 0), res);
		}


		[TestMethod]
		public void ToDate_DefaultFormat_DateNotMatchedIt_DoesNotConverts ()
		{
			var res = "05.01.2000 14:00:00".ToDate ("dd.MM.yyyy HH:mm");

			Assert.IsNull (res);
		}


		[TestMethod]
		public void ToDateTime_DefaultFormat_Converts ()
		{
			var res = "01.02.3000 04:05:06".ToDateTime ();

			Assert.IsNotNull (res);
			Assert.AreEqual (new DateTime (3000, 2, 1, 4, 5, 6), res);
		}


		[TestMethod]
		public void ToDateTime_DefaultFormat_DateOnly_Converts ()
		{
			var res = "05.01.2000".ToDateTime ();

			Assert.IsNotNull (res);
			Assert.AreEqual (new DateTime (2000, 1, 5), res);
		}


		[TestMethod]
		public void ToDateTime_CustomFormat_Converts ()
		{
			var res = "05.01.2000 14:00".ToDateTime ("dd.MM.yyyy HH:mm");

			Assert.IsNotNull (res);
			Assert.AreEqual (new DateTime (2000, 1, 5, 14, 0, 0), res);
		}


		[TestMethod]
		public void ToDateTime_CustomFormat_DateNotMatchedIt_DoesNotConverts ()
		{
			var res = "05.01.2000 14:00:00".ToDateTime ("dd.MM.yyyy HH:mm");

			Assert.IsNull (res);
		}


        [TestMethod]
		public void ToFloat_NumberWithDot_Converts ()
        {
            var res = "1.23".ToFloat ();

            res.Should ().Be (1.23F);
        }


        [TestMethod]
		public void ToFloat_NumberWithComma_Converts ()
        {
            var res = "1,23".ToFloat ();

            res.Should ().Be (1.23F);
        }


        [TestMethod]
		public void ToDouble_NumberWithDot_Converts ()
        {
            var res = "1.23".ToDouble ();

            res.Should ().Be (1.23);
        }


        [TestMethod]
		public void ToDouble_NumberWithComma_Converts ()
        {
            var res = "1,23".ToDouble ();

            res.Should ().Be (1.23);
        }
	}
}
