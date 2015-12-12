using FluentAssertions;
using Xunit;

namespace Rocks.Helpers.Tests
{
    public class StringExtensionsTests
    {
        [Fact]
        public void SplitToTrimmedNonEmptyStringList_SplitsCorrectly ()
        {
            // act
            var result = "a.1, b, c 2".SplitToTrimmedNonEmptyStringList ();

            // assert
            result.Should ().Equal ("a.1", "b", "c 2");
        }


        [Theory]
        [InlineData (null, null, null)]
        [InlineData (null, "", null)]
        [InlineData ("", null, null)]
        [InlineData ("", "", null)]
        [InlineData ("a", "", null)]
        [InlineData ("a", "b", null)]
        [InlineData ("ab", "b", null)]
        [InlineData ("abc", "b", "c")]
        [InlineData ("bc", "b", "c")]
        [InlineData ("abcb", "b", null)]
        [InlineData ("abcbdef", "b", "def")]
        public void RightPart_Theory_Comply (string source, string search, string expected)
        {
            // act
            var result = source.RightPart (search);

            // assert
            result.Should ().Be (expected);
        }


        [Theory]
        [InlineData (null, null, null)]
        [InlineData (null, "", null)]
        [InlineData ("", null, null)]
        [InlineData ("", "", null)]
        [InlineData ("a", "", null)]
        [InlineData ("a", "b", null)]
        [InlineData ("ba", "b", null)]
        [InlineData ("abc", "b", "a")]
        [InlineData ("ab", "b", "a")]
        [InlineData ("babc", "b", null)]
        [InlineData ("efbcbda", "b", "ef")]
        public void LeftPart_Theory_Comply (string source, string search, string expected)
        {
            // act
            var result = source.LeftPart (search);

            // assert
            result.Should ().Be (expected);
        }
    }
}