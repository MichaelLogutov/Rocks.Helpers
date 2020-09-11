using System;
using FluentAssertions;
using Xunit;

namespace Rocks.Helpers.Tests
{
    public class StringConvertExtensionsTests
    {
        [Fact]
        public void ToDate_DefaultFormat_Converts()
        {
            var res = "01.02.3000".ToDate();

            Assert.NotNull(res);
            Assert.Equal(new DateTime(3000, 2, 1), res);
        }


        [Fact]
        public void ToDate_CustomFormat_Converts()
        {
            var res = "05.01.2000 14:00".ToDate("dd.MM.yyyy HH:mm");

            Assert.NotNull(res);
            Assert.Equal(new DateTime(2000, 1, 5, 14, 0, 0), res);
        }


        [Fact]
        public void ToDate_DefaultFormat_DateNotMatchedIt_DoesNotConverts()
        {
            var res = "05.01.2000 14:00:00".ToDate("dd.MM.yyyy HH:mm");

            Assert.Null(res);
        }


        [Fact]
        public void ToDateTime_DefaultFormat_Converts()
        {
            var res = "01.02.3000 04:05:06".ToDateTime();

            Assert.NotNull(res);
            Assert.Equal(new DateTime(3000, 2, 1, 4, 5, 6), res);
        }


        [Fact]
        public void ToDateTime_DefaultFormat_DateOnly_Converts()
        {
            var res = "05.01.2000".ToDateTime();

            Assert.NotNull(res);
            Assert.Equal(new DateTime(2000, 1, 5), res);
        }


        [Fact]
        public void ToDateTime_CustomFormat_Converts()
        {
            var res = "05.01.2000 14:00".ToDateTime("dd.MM.yyyy HH:mm");

            Assert.NotNull(res);
            Assert.Equal(new DateTime(2000, 1, 5, 14, 0, 0), res);
        }


        [Fact]
        public void ToDateTime_CustomFormat_DateNotMatchedIt_DoesNotConverts()
        {
            var res = "05.01.2000 14:00:00".ToDateTime("dd.MM.yyyy HH:mm");

            Assert.Null(res);
        }


        [Fact]
        public void ToFloat_NumberWithDot_Converts()
        {
            var res = "1.23".ToFloat();

            res.Should().Be(1.23F);
        }


        [Fact]
        public void ToFloat_NumberWithComma_Converts()
        {
            var res = "1,23".ToFloat();

            res.Should().Be(1.23F);
        }


        [Fact]
        public void ToDouble_NumberWithDot_Converts()
        {
            var res = "1.23".ToDouble();

            res.Should().Be(1.23);
        }


        [Fact]
        public void ToDouble_NumberWithComma_Converts()
        {
            var res = "1,23".ToDouble();

            res.Should().Be(1.23);
        }


        [Fact]
        public void ToDecimal_NumberWithDot_Converts()
        {
            var res = "1.23".ToDecimal();

            res.Should().Be(1.23M);
        }


        [Fact]
        public void ToDecimal_NumberWithComma_Converts()
        {
            var res = "1,23".ToDecimal();

            res.Should().Be(1.23M);
        }


        [Fact]
        public void ToEnum_Null_ReturnsDefault()
        {
            var res = ((string) null).ToEnum(TestEnum.Value2);

            res.Should().Be(TestEnum.Value2);
        }


        [Fact]
        public void ToEnum_Empty_ReturnsDefault()
        {
            var res = string.Empty.ToEnum(TestEnum.Value2);

            res.Should().Be(TestEnum.Value2);
        }


        [Fact]
        public void ToEnum_Value_ReturnsCorrespondingEnumValue()
        {
            var res = "Value3".ToEnum(TestEnum.Value2);

            res.Should().Be(TestEnum.Value3);
        }


        [Fact]
        public void ToEnum_InvalidValue_ReturnsDefault()
        {
            var res = "Value4".ToEnum(TestEnum.Value2);

            res.Should().Be(TestEnum.Value2);
        }


        [Fact]
        public void ToEnum_Value_WrongCase_ByDefault_ReturnsCorrespondingEnumValue()
        {
            var res = "value3".ToEnum(TestEnum.Value2);

            res.Should().Be(TestEnum.Value3);
        }


        [Fact]
        public void ToEnum_Value_WrongCase_CaseSensitive_ReturnsDefault()
        {
            var res = "value3".ToEnum(TestEnum.Value2, ignoreCase: false);

            res.Should().Be(TestEnum.Value2);
        }


        [Fact]
        public void ToEnumNullable_Null_ReturnsDefault()
        {
            var res = ((string) null).ToEnum<TestEnum>(null);

            res.Should().BeNull();
        }


        [Fact]
        public void ToEnumNullable_Empty_ReturnsDefault()
        {
            var res = string.Empty.ToEnum<TestEnum>(null);

            res.Should().BeNull();
        }


        [Fact]
        public void ToEnumNullable_Value_ReturnsCorrespondingEnumValue()
        {
            var res = "Value3".ToEnum<TestEnum>(null);

            res.Should().Be(TestEnum.Value3);
        }


        [Fact]
        public void ToEnumNullable_InvalidValue_ReturnsDefault()
        {
            var res = "Value4".ToEnum<TestEnum>(null);

            res.Should().BeNull();
        }


        [Fact]
        public void ToEnumNullable_Value_WrongCase_ByDefault_ReturnsCorrespondingEnumValue()
        {
            var res = "value3".ToEnum<TestEnum>(null);

            res.Should().Be(TestEnum.Value3);
        }


        [Fact]
        public void ToEnumNullable_Value_WrongCase_CaseSensitive_ReturnsDefault()
        {
            var res = "value3".ToEnum<TestEnum>(null, ignoreCase: false);

            res.Should().BeNull();
        }
    }
}