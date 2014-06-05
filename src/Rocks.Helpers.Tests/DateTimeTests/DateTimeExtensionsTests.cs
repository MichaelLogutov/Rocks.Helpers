using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable RedundantArgumentDefaultValue

namespace Rocks.Helpers.Tests.DateTimeTests
{
	[TestClass]
	public class DateTimeExtensionsTests
	{
		[TestMethod]
		public void GetQuarter_ReturnsCorrectResult ()
		{
			new DateTime (2000, 1, 1).GetQuarter ().Should ().Be (1);
			new DateTime (2000, 3, 31).GetQuarter ().Should ().Be (1);

			new DateTime (2000, 4, 1).GetQuarter ().Should ().Be (2);
			new DateTime (2000, 6, 30).GetQuarter ().Should ().Be (2);

			new DateTime (2000, 7, 1).GetQuarter ().Should ().Be (3);
			new DateTime (2000, 9, 30).GetQuarter ().Should ().Be (3);

			new DateTime (2000, 10, 1).GetQuarter ().Should ().Be (4);
			new DateTime (2000, 12, 31).GetQuarter ().Should ().Be (4);
		}


		[TestMethod]
		public void EndOfTheDay_EndOfTheDay_ReturnsTheSameDate ()
		{
			var date = new DateTime (2000, 1, 1, 23, 59, 59);

			date.EndOfTheDay ().Should ().Be (date);
		}


		[TestMethod]
		public void EndOfTheDay_StartOfTheDay_ReturnsEndOfTheDay ()
		{
			new DateTime (2000, 1, 1).EndOfTheDay ().Should ().Be (new DateTime (2000, 1, 1, 23, 59, 59));
		}


		[TestMethod]
		public void DayOfTheWeek_StartOfTheWeek_Returns1 ()
		{
			new DateTime (2014, 1, 20).DayOfTheWeek (DayOfWeek.Monday).Should ().Be (1);
		}


		[TestMethod]
		public void DayOfTheWeek_EndOfTheWeek_Returns7 ()
		{
			new DateTime (2014, 1, 26).DayOfTheWeek (DayOfWeek.Monday).Should ().Be (7);
		}


		[TestMethod]
		public void StartOfTheWeek_StartOfTheWeek_ReturnsTheSameDate ()
		{
			var date = new DateTime (2014, 1, 20);

			date.StartOfTheWeek (DayOfWeek.Monday).Should ().Be (date);
		}


		[TestMethod]
		public void StartOfTheWeek_EndOfTheWeek_ReturnsStartOfTheWeek ()
		{
			new DateTime (2014, 1, 26).StartOfTheWeek (DayOfWeek.Monday).Should ().Be (new DateTime (2014, 1, 20));
		}


		[TestMethod]
		public void EndOfTheWeek_EndOfTheWeek_ReturnsTheSameDate ()
		{
			var date = new DateTime (2014, 1, 26);

			date.EndOfTheWeek (DayOfWeek.Monday).Should ().Be (date);
		}


		[TestMethod]
		public void EndOfTheWeek_StartOfTheWeek_ReturnsEndOfTheWeek ()
		{
			new DateTime (2014, 1, 20).EndOfTheWeek (DayOfWeek.Monday).Should ().Be (new DateTime (2014, 1, 26));
		}


		[TestMethod]
		public void StartOfTheMonth_ReturnsStartOfTheMonth ()
		{
			new DateTime (2014, 1, 20).StartOfTheMonth ().Should ().Be (new DateTime (2014, 1, 1));
		}


		[TestMethod]
		public void EndOfTheMonth_ReturnsEndOfTheMonth ()
		{
			new DateTime (2014, 1, 30).EndOfTheMonth ().Should ().Be (new DateTime (2014, 1, 31));
		}


		[TestMethod]
		public void StartOfTheYear_ReturnsStartOfTheYear ()
		{
			new DateTime (2014, 7, 12).StartOfTheYear ().Should ().Be (new DateTime (2014, 1, 1));
		}


		[TestMethod]
		public void EndOfTheYear_ReturnsEndOfTheYear ()
		{
			new DateTime (2014, 7, 12).EndOfTheYear ().Should ().Be (new DateTime (2014, 12, 31));
		}
	}
}