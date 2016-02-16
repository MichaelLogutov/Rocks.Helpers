using System;
using System.Globalization;
using FluentAssertions;
using Xunit;

// ReSharper disable RedundantArgumentDefaultValue

namespace Rocks.Helpers.Tests.DateTimeTests
{
    public class DateTimeExtensionsTests
    {
        [Theory]
        [InlineData ("01.01.2000", 1)]
        [InlineData ("31.01.2000", 1)]
        [InlineData ("01.04.2000", 2)]
        [InlineData ("30.06.2000", 2)]
        [InlineData ("01.07.2000", 3)]
        [InlineData ("30.09.2000", 3)]
        [InlineData ("01.10.2000", 4)]
        [InlineData ("31.12.2000", 4)]
        public void GetQuarter_ReturnsCorrectResult (string d, int expected)
        {
            // arrange


            // act
            var result = d.AsTestDateTime ().GetQuarter ();


            // assert
            result.Should ().Be (expected);
        }


        [Theory]
        [InlineData ("01.10.2000", 1, "01.01.2000")]
        [InlineData ("01.01.2001", 2, "01.04.2001")]
        [InlineData ("20.07.2015", 3, "01.07.2015")]
        [InlineData ("31.12.2002", 4, "01.10.2002")]
        public void StartOfTheQuarter_ReturnsCorrectResult (string d, int quarter, string expected)
        {
            // arrange

            // act
            var result = d.AsTestDateTime ().StartOfTheQuarter (quarter);


            // assert
            result.Should ().Be (expected.AsTestDateTime ());
        }


        [Theory]
        [InlineData ("10.01.2000", "01.01.2000")]
        [InlineData ("01.04.2001", "01.04.2001")]
        [InlineData ("20.07.2015", "01.07.2015")]
        [InlineData ("31.12.2002", "01.10.2002")]
        public void StartOfTheQuarter_Current_ReturnsCorrectResult (string d, string expected)
        {
            // arrange

            // act
            var result = d.AsTestDateTime ().StartOfTheQuarter ();


            // assert
            result.Should ().Be (expected.AsTestDateTime ());
        }


        [Theory]
        [InlineData ("01.10.2000", 1, "31.03.2000")]
        [InlineData ("01.01.2001", 2, "30.06.2001")]
        [InlineData ("20.07.2015", 3, "30.09.2015")]
        [InlineData ("31.12.2002", 4, "31.12.2002")]
        public void EndOfTheQuarter_ReturnsCorrectResult (string d, int quarter, string expected)
        {
            // arrange

            // act
            var result = d.AsTestDateTime ().EndOfTheQuarter (quarter);


            // assert
            result.Should ().Be (expected.AsTestDateTime ());
        }


        [Theory]
        [InlineData ("02.02.2000", "31.03.2000")]
        [InlineData ("01.04.2001", "30.06.2001")]
        [InlineData ("20.07.2015", "30.09.2015")]
        [InlineData ("31.12.2002", "31.12.2002")]
        public void EndOfTheQuarter_Current_ReturnsCorrectResult (string d, string expected)
        {
            // arrange

            // act
            var result = d.AsTestDateTime ().EndOfTheQuarter ();


            // assert
            result.Should ().Be (expected.AsTestDateTime ());
        }


        [Fact]
        public void EndOfTheDay_EndOfTheDay_ReturnsTheSameDate ()
        {
            var date = new DateTime (2000, 1, 1, 23, 59, 59);

            date.EndOfTheDay ().Should ().Be (date);
        }


        [Fact]
        public void EndOfTheDay_StartOfTheDay_ReturnsEndOfTheDay ()
        {
            new DateTime (2000, 1, 1).EndOfTheDay ().Should ().Be (new DateTime (2000, 1, 1, 23, 59, 59));
        }


        [Theory]
        [InlineData ("20.01.2014", DayOfWeek.Monday, 1)]
        [InlineData ("26.01.2014", DayOfWeek.Monday, 7)]
        [InlineData ("03.01.2016", DayOfWeek.Monday, 7)]
        [InlineData ("03.01.2016", DayOfWeek.Sunday, 1)]
        public void DayOfTheWeek_Theory_Comply (string date, DayOfWeek dayOfWeek, int expected)
        {
            date.AsTestDateTime ().DayOfTheWeek (dayOfWeek).Should ().Be (expected);
        }


        [Fact]
        public void StartOfTheWeek_StartOfTheWeek_ReturnsTheSameDate ()
        {
            var date = new DateTime (2014, 1, 20);

            date.StartOfTheWeek (DayOfWeek.Monday).Should ().Be (date);
        }


        [Fact]
        public void StartOfTheWeek_EndOfTheWeek_ReturnsStartOfTheWeek ()
        {
            new DateTime (2014, 1, 26).StartOfTheWeek (DayOfWeek.Monday).Should ().Be (new DateTime (2014, 1, 20));
        }


        [Fact]
        public void EndOfTheWeek_EndOfTheWeek_ReturnsTheSameDate ()
        {
            var date = new DateTime (2014, 1, 26);

            date.EndOfTheWeek (DayOfWeek.Monday).Should ().Be (date);
        }


        [Fact]
        public void EndOfTheWeek_StartOfTheWeek_ReturnsEndOfTheWeek ()
        {
            new DateTime (2014, 1, 20).EndOfTheWeek (DayOfWeek.Monday).Should ().Be (new DateTime (2014, 1, 26));
        }


        [Fact]
        public void StartOfTheMonth_ReturnsStartOfTheMonth ()
        {
            new DateTime (2014, 1, 20).StartOfTheMonth ().Should ().Be (new DateTime (2014, 1, 1));
        }


        [Fact]
        public void EndOfTheMonth_ReturnsEndOfTheMonth ()
        {
            new DateTime (2014, 1, 30).EndOfTheMonth ().Should ().Be (new DateTime (2014, 1, 31));
        }


        [Fact]
        public void StartOfTheYear_ReturnsStartOfTheYear ()
        {
            new DateTime (2014, 7, 12).StartOfTheYear ().Should ().Be (new DateTime (2014, 1, 1));
        }


        [Fact]
        public void EndOfTheYear_ReturnsEndOfTheYear ()
        {
            new DateTime (2014, 7, 12).EndOfTheYear ().Should ().Be (new DateTime (2014, 12, 31));
        }
    }
}