using FluentAssertions;
using Xunit;

namespace Rocks.Helpers.Tests
{
	public class StringExtensionsTests
	{
		[Fact]
		public void ShouldSplitToTrimmedNonEmptyStringList ()
		{
            // act
			var result = "a.1, b, c 2".SplitToTrimmedNonEmptyStringList ();

            // assert
            result.Should ().Equal ("a.1", "b", "c 2");
		}
	}
}



