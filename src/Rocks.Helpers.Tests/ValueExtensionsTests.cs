using System;
using FluentAssertions;
using Xunit;

namespace Rocks.Helpers.Tests
{
    public class ValueExtensionsTests
    {
#pragma warning disable IDE1006 // Naming Styles
        public enum TestEnum
        {
            A = 1,
            B = 2
        }
#pragma warning restore IDE1006 // Naming Styles


        [Fact]
        public void ShouldRequireEnum_Ok()
        {
            var target = TestEnum.A;

            target.RequiredEnum("target");
        }


        [Fact]
        public void ShouldRequireEnum_OkNullable()
        {
            var target = (TestEnum?) TestEnum.A;

            target.RequiredEnum("target");
        }


        [Fact]
        public void ShouldRequireEnum_FailWhenInvalid()
        {
            var target = (TestEnum) 0;

            Action act = () => target.RequiredEnum("target");

            act.Should().Throw<ArgumentOutOfRangeException>();
        }


        [Fact]
        public void ShouldRequiredAll_Ok()
        {
            var target = new[] { TestEnum.A, TestEnum.B };

            target.RequiredAll(x => x.RequiredEnum("target"));
        }


        [Fact]
        public void ShouldRequiredAll_FailWhenInvalid()
        {
            var target = new[] { TestEnum.A, (TestEnum) 0 };

            Action act = () => target.RequiredAll(x => x.RequiredEnum("target"));

            act.Should().Throw<ArgumentOutOfRangeException>();
        }


        [Fact]
        public void AsValidEntityUrl_ValidUrl_DoesNotThrows()
        {
            // arrange
            var url = "aaa";


            // act
            var action = new Action(() => url.AsValidEntityUrl("url"));


            // assert
            action.Should().NotThrow();
        }


        [Fact]
        public void AsValidEntityUrl_ValidUrlWithRussianChars_DoesNotThrows()
        {
            // arrange
            var url = "абв";


            // act
            var action = new Action(() => url.AsValidEntityUrl("url"));


            // assert
            action.Should().NotThrow();
        }


        [Fact]
        public void AsValidEntityUrl_Null_DoesNotThrows()
        {
            // arrange
            string url = null;


            // act
            // ReSharper disable once ExpressionIsAlwaysNull
            var action = new Action(() => url.AsValidEntityUrl("url"));


            // assert
            action.Should().NotThrow();
        }


        [Fact]
        public void AsValidEntityUrl_InvalidUrl_Throws()
        {
            // arrange
            var url = "+aaa";


            // act
            var action = new Action(() => url.AsValidEntityUrl("url"));


            // assert
            action.Should().Throw<FormatException>();
        }


        [Fact]
        public void AsValidEmail_ValidEmail_DoesNotThrows()
        {
            // arrange
            var email = "a@a";


            // act
            var action = new Action(() => email.AsValidEmail("email"));


            // assert
            action.Should().NotThrow();
        }


        [Fact]
        public void AsValidEmail_InvalidEmail_Throws()
        {
            // arrange
            var email = "aaa";


            // act
            var action = new Action(() => email.AsValidEmail("email"));


            // assert
            action.Should().Throw<FormatException>();
        }
    }
}