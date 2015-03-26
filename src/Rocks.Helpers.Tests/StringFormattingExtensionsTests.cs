using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rocks.Helpers.Tests
{
    [TestClass]
    public class StringFormattingExtensionsTests
    {
        [TestMethod]
        public void TimeSpan_ToFormattedString_Milliseconds_ReturnsMilliseconds ()
        {
            new TimeSpan (0, 0, 0, 0, 123).ToFormattedString ().Should ().Be ("123 ms");
        }


        [TestMethod]
        public void TimeSpan_ToFormattedString_SecondsAndMilliseconds_ReturnsSecondsAndMilliseconds ()
        {
            new TimeSpan (0, 0, 0, 45, 123).ToFormattedString ().Should ().Be ("45 sec 123 ms");
        }


        [TestMethod]
        public void TimeSpan_ToFormattedString_Seconds_ReturnsSeconds ()
        {
            new TimeSpan (0, 0, 0, 45, 0).ToFormattedString ().Should ().Be ("45 sec");
        }


        [TestMethod]
        public void TimeSpan_ToFormattedString_MinutesSecondsAndMilliseconds_ReturnsMinutesSecondsAndMilliseconds ()
        {
            new TimeSpan (0, 0, 7, 45, 123).ToFormattedString ().Should ().Be ("7 min 45 sec 123 ms");
        }


        [TestMethod]
        public void TimeSpan_ToFormattedString_MinutesAndMilliseconds_NoSeconds_ReturnsMinutesAndMilliseconds ()
        {
            new TimeSpan (0, 0, 7, 0, 123).ToFormattedString ().Should ().Be ("7 min 123 ms");
        }


        [TestMethod]
        public void TimeSpan_ToFormattedString_HoursMinutesSecondsAndMilliseconds_ReturnsHoursMinutesSecondsAndMilliseconds ()
        {
            new TimeSpan (0, 8, 7, 45, 123).ToFormattedString ().Should ().Be ("8 h 7 min 45 sec 123 ms");
        }


        [TestMethod]
        public void TimeSpan_ToFormattedString_DaysHoursMinutesSecondsAndMilliseconds_ReturnsDaysHoursMinutesSecondsAndMilliseconds ()
        {
            new TimeSpan (9, 8, 7, 45, 123).ToFormattedString ().Should ().Be ("9 d 8 h 7 min 45 sec 123 ms");
        }


        [TestMethod]
        public void TimeSpan_ToFormattedString_Zero_ReturnsEmptyString ()
        {
            TimeSpan.Zero.ToFormattedString ().Should ().Be (string.Empty);
        }


        [TestMethod]
        public void TimeSpan_ToFormattedString_Top2_Milliseconds_ReturnsMilliseconds ()
        {
            new TimeSpan (0, 0, 0, 0, 123).ToFormattedString (2).Should ().Be ("123 ms");
        }


        [TestMethod]
        public void TimeSpan_ToFormattedString_Top1_SecondsAndMilliseconds_ReturnsSecondsAndMilliseconds ()
        {
            new TimeSpan (0, 0, 0, 45, 123).ToFormattedString (2).Should ().Be ("45 sec 123 ms");
        }


        [TestMethod]
        public void TimeSpan_ToFormattedString_Top1_SecondsAndMilliseconds_ReturnsSeconds ()
        {
            new TimeSpan (0, 0, 0, 45, 123).ToFormattedString (1).Should ().Be ("45 sec");
        }


        [TestMethod]
        public void TimeSpan_ToFormattedString_Top2_Seconds_ReturnsSeconds ()
        {
            new TimeSpan (0, 0, 0, 45, 0).ToFormattedString ().Should ().Be ("45 sec");
        }


        [TestMethod]
        public void TimeSpan_ToFormattedString_Top2_MinutesSecondsAndMilliseconds_ReturnsMinutesAndSeconds ()
        {
            new TimeSpan (0, 0, 7, 45, 123).ToFormattedString (2).Should ().Be ("7 min 45 sec");
        }


        [TestMethod]
        public void TimeSpan_ToFormattedString_Top2_MinutesAndMilliseconds_NoSeconds_ReturnsMinutes ()
        {
            new TimeSpan (0, 0, 7, 0, 123).ToFormattedString (2).Should ().Be ("7 min");
        }


        [TestMethod]
        public void TimeSpan_ToFormattedString_Top3_HoursMinutesSecondsAndMilliseconds_ReturnsHoursMinutesAndSeconds ()
        {
            new TimeSpan (0, 8, 7, 45, 123).ToFormattedString (3).Should ().Be ("8 h 7 min 45 sec");
        }


        [TestMethod]
        public void TimeSpan_ToFormattedString_Top2_HoursMinutesAndMilliseconds_ReturnsHoursAndMinutes ()
        {
            new TimeSpan (0, 8, 7, 45, 123).ToFormattedString (2).Should ().Be ("8 h 7 min");
        }


        [TestMethod]
        public void TimeSpan_ToFormattedString_Top5_DaysHoursMinutesSecondsAndMilliseconds_ReturnsDaysHoursMinutesSecondsAndMilliseconds ()
        {
            new TimeSpan (9, 8, 7, 45, 123).ToFormattedString (5).Should ().Be ("9 d 8 h 7 min 45 sec 123 ms");
        }
    }
}