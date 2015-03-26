using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rocks.Helpers.Tests
{
	[TestClass]
	public class StringFormattingExtensionsTests
	{
		[TestMethod]
		public void TimeSpan_ToFormattedString_LessThanSecond_ReturnsMilliseconds ()
		{
			new TimeSpan (0, 0, 0, 0, 123).ToFormattedString ().Should ().Be ("123 ms");
		}


		[TestMethod]
		public void TimeSpan_ToFormattedString_LessThanMinute_ReturnsSecondsAndMilliseconds ()
		{
			new TimeSpan (0, 0, 0, 45, 123).ToFormattedString ().Should ().Be ("45 sec 123 ms");
		}


		[TestMethod]
		public void TimeSpan_ToFormattedString_LessThanHour_ReturnsMinutesAndSeconds ()
		{
			new TimeSpan (0, 0, 7, 45, 123).ToFormattedString ().Should ().Be ("7 min 45 sec");
		}

		[TestMethod]
		public void TimeSpan_ToFormattedString_LessThanADay_ReturnsHoursAndMinutes ()
		{
			new TimeSpan (0, 8, 7, 45, 123).ToFormattedString ().Should ().Be ("8 h 7 min");
		}

		[TestMethod]
		public void TimeSpan_ToFormattedString_MoreThanADay_ReturnsDaysAndHoursAndMinutes ()
		{
			new TimeSpan (9, 8, 7, 45, 123).ToFormattedString ().Should ().Be ("9 d 8 h 7 min");
		}
	}
}