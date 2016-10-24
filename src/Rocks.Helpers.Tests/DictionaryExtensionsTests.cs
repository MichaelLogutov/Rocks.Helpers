using System.Linq;
using FluentAssertions;
using Xunit;

namespace Rocks.Helpers.Tests
{
    public class DictionaryExtensionsTests
    {
        [Fact]
        public void GetValueOrDefault_Always_WorksWithDictionary()
        {
            // arrange
            var sut = "abc".ToDictionary(k => k);

            // act
            var result = sut.GetValueOrDefault('z', '_');


            // assert
            result.Should().Be('_');
        }
    }
}