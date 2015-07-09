using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rocks.Helpers.Tests
{
    [TestClass]
    public class RangeExtensionsTests
    {
        [TestMethod]
        public void Clamp_ValueWithinRange_ReturnsValue ()
        {
            // arrange


            // act
            var result = 15.Clamp (10, 20);


            // assert
            result.Should ().Be (15);
        }


        [TestMethod]
        public void Clamp_ValueLessThanMin_ReturnsMin ()
        {
            // arrange


            // act
            var result = 1.Clamp (10, 20);


            // assert
            result.Should ().Be (10);
        }


        [TestMethod]
        public void Clamp_ValueGreaterThanMax_ReturnsMax ()
        {
            // arrange


            // act
            var result = 30.Clamp (10, 20);


            // assert
            result.Should ().Be (20);
        }


        [TestMethod]
        public void Min_ValueGreaterThanMin_ReturnsValue ()
        {
            // arrange


            // act
            var result = 15.Min (10);


            // assert
            result.Should ().Be (15);
        }


        [TestMethod]
        public void Min_ValueLessThanMin_ReturnsMin ()
        {
            // arrange


            // act
            var result = 1.Min (10);


            // assert
            result.Should ().Be (10);
        }


        [TestMethod]
        public void Max_ValueLessThanMax_ReturnsValue ()
        {
            // arrange


            // act
            var result = 15.Max (20);


            // assert
            result.Should ().Be (15);
        }


        [TestMethod]
        public void Max_ValueGreaterThanMax_ReturnsMax ()
        {
            // arrange


            // act
            var result = 20.Max (10);


            // assert
            result.Should ().Be (10);
        }
    }
}