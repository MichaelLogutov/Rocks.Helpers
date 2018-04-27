﻿using System;
using FluentAssertions;
using Xunit;

namespace Rocks.Helpers.Tests.DateTimeTests
{
	public class SplitToWeeksTests
	{
		[Fact]
		public void SplitToWeeks_OneDate_ReturnsOneWeekSameDates ()
		{
			// arrange
			var date = new DateTime (2014, 2, 13);
			var date2 = new DateTime (2014, 2, 14);


			// act
			var result = date.SplitToWeeks (date2);


			// assert
			result.Should().BeEquivalentTo (new[]
			                                {
				                                new Tuple<DateTime, DateTime> (date, date2)
			                                });
		}


		[Fact]
		public void SplitToWeeks_LessThanAWeek_ReturnsOneWeekSameDates ()
		{
			// arrange
			var date = new DateTime (2014, 2, 11);
			var date2 = new DateTime (2014, 2, 15);


			// act
			var result = date.SplitToWeeks (date2);


			// assert
			result.Should().BeEquivalentTo (new[]
			                                {
				                                new Tuple<DateTime, DateTime> (date, date2)
			                                });
		}


		[Fact]
		public void SplitToWeeks_OneWholeWeek_ReturnsOneWeekSameDates ()
		{
			// arrange
			var date = new DateTime (2014, 2, 10);
			var date2 = new DateTime (2014, 2, 16);


			// act
			var result = date.SplitToWeeks (date2);


			// assert
			result.Should().BeEquivalentTo (new[]
			                                {
				                                new Tuple<DateTime, DateTime> (date, date2)
			                                });
		}


		[Fact]
		public void SplitToWeeks_OneWeekStartingInMid_ReturnsOneWeekSameDates ()
		{
			// arrange
			var date = new DateTime (2014, 2, 14);
			var date2 = new DateTime (2014, 2, 16);


			// act
			var result = date.SplitToWeeks (date2);


			// assert
			result.Should().BeEquivalentTo (new[]
			                                {
				                                new Tuple<DateTime, DateTime> (date, date2)
			                                });
		}


		[Fact]
		public void SplitToWeeks_OneWeekEndingInMid_ReturnsOneWeekSameDates ()
		{
			// arrange
			var date = new DateTime (2014, 2, 10);
			var date2 = new DateTime (2014, 2, 14);


			// act
			var result = date.SplitToWeeks (date2);


			// assert
			result.Should().BeEquivalentTo (new[]
			                                {
				                                new Tuple<DateTime, DateTime> (date, date2)
			                                });
		}


		[Fact]
		public void SplitToWeeks_TwoWeeksStartingAndEndingInMid_ReturnsTwoWeeksWithSameDates ()
		{
			// arrange
			var date = new DateTime (2014, 2, 6);
			var date2 = new DateTime (2014, 2, 14);


			// act
			var result = date.SplitToWeeks (date2);


			// assert
			result.Should().BeEquivalentTo (new[]
			                                {
				                                new Tuple<DateTime, DateTime> (new DateTime (2014, 2, 6), new DateTime (2014, 2, 9)),
												new Tuple<DateTime, DateTime> (new DateTime (2014, 2, 10), new DateTime (2014, 2, 14))
			                                });
		}
	}
}


