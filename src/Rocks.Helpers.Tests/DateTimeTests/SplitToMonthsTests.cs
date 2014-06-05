using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rocks.Helpers.Tests.DateTimeTests
{
	[TestClass]
	public class SplitToMonthsTests
	{
		[TestMethod]
		public void SplitToMonths_OneDate_ReturnsOneMonthSameDates ()
		{
			// arrange
			var date = new DateTime (2000, 1, 30);
			var date2 = new DateTime (2000, 1, 31);


			// act
			var result = date.SplitToMonths (date2);


			// assert
			result.ShouldAllBeEquivalentTo (new[]
			                                {
				                                new Tuple<DateTime, DateTime> (date, date2)
			                                });
		}


		[TestMethod]
		public void SplitToMonths_LessThanAMonth_ReturnsOneMonthSameDates ()
		{
			// arrange
			var date = new DateTime (2000, 1, 1);
			var date2 = new DateTime (2000, 1, 14);


			// act
			var result = date.SplitToMonths (date2);


			// assert
			result.ShouldAllBeEquivalentTo (new[]
			                                {
				                                new Tuple<DateTime, DateTime> (date, date2)
			                                });
		}


		[TestMethod]
		public void SplitToMonths_OneWholeMonth_ReturnsOneMonthSameDates ()
		{
			// arrange
			var date = new DateTime (2000, 1, 1);
			var date2 = new DateTime (2000, 1, 31);


			// act
			var result = date.SplitToMonths (date2);


			// assert
			result.ShouldAllBeEquivalentTo (new[]
			                                {
				                                new Tuple<DateTime, DateTime> (date, date2)
			                                });
		}


		[TestMethod]
		public void SplitToMonths_OneMonthStartingInMid_ReturnsOneMonthSameDates ()
		{
			// arrange
			var date = new DateTime (2000, 1, 14);
			var date2 = new DateTime (2000, 1, 31);


			// act
			var result = date.SplitToMonths (date2);


			// assert
			result.ShouldAllBeEquivalentTo (new[]
			                                {
				                                new Tuple<DateTime, DateTime> (date, date2)
			                                });
		}


		[TestMethod]
		public void SplitToMonths_OneMonthEndingInMid_ReturnsOneMonthSameDates ()
		{
			// arrange
			var date = new DateTime (2000, 1, 1);
			var date2 = new DateTime (2000, 1, 14);


			// act
			var result = date.SplitToMonths (date2);


			// assert
			result.ShouldAllBeEquivalentTo (new[]
			                                {
				                                new Tuple<DateTime, DateTime> (date, date2)
			                                });
		}


		[TestMethod]
		public void SplitToMonths_TwoMonthsStartingAndEndingInMid_ReturnsTwoMonthsWithSameDates ()
		{
			// arrange
			var date = new DateTime (2000, 1, 14);
			var date2 = new DateTime (2000, 2, 20);


			// act
			var result = date.SplitToMonths (date2);


			// assert
			result.ShouldAllBeEquivalentTo (new[]
			                                {
				                                new Tuple<DateTime, DateTime> (new DateTime (2000, 1, 14), new DateTime (2000, 1, 31)),
												new Tuple<DateTime, DateTime> (new DateTime (2000, 2, 1), new DateTime (2000, 2, 20))
			                                });
		}
	}
}