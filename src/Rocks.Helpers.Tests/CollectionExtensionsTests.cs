using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FluentAssertions;
using Xunit;

// ReSharper disable ExpressionIsAlwaysNull

namespace Rocks.Helpers.Tests
{
    public class CollectionExtensionsTests
    {
        [Fact]
        public void SkipNull_OneItemNull_ReturnsNoNulls ()
        {
            var result = (new[] { "a", null, "b" }).SkipNull ();

            result.Should ().Equal ("a", "b");
        }


        [Fact]
        public void SkipNull_Null_ReturnsEmpty ()
        {
            var result = ((string[]) null).SkipNull ();

            result.Should ().BeEmpty ();
        }


        [Fact]
        public void SkipNullOrEmpty_OneItemEmptyString_AndOneItemNull_ReturnsNoNullOrEmpty ()
        {
            var result = (new[] { "a", null, "b", string.Empty }).SkipNullOrEmpty ();

            result.Should ().Equal ("a", "b");
        }


        [Fact]
        public void ShouldSkipNullOrEmpty_Null_ReturnsEmpty ()
        {
            var result = ((string[]) null).SkipNullOrEmpty ();

            result.Should ().BeEmpty ();
        }


        [Fact]
        public void TrimAll_ReturnsAllTrimmed ()
        {
            var result = (new[] { "a", "b ", " c" }).TrimAll ();

            result.Should ().Equal ("a", "b", "c");
        }


        [Fact]
        public void TrimAll_Null_ReturnsEmpty ()
        {
            var result = ((string[]) null).TrimAll ();

            result.Should ().BeEmpty ();
        }


        [Fact]
        public void ConvertToList_ReturnsIList ()
        {
            // arrange
            var data = new[] { "a", null, "b" };


            // act
            var result = data.Select (x => x).ConvertToList ();


            // assert
            result.Should ()
                  .BeAssignableTo<IList<string>> ()
                  .And
                  .NotBeSameAs (data)
                  .And
                  .Equal ("a", null, "b");
        }


        [Fact]
        public void ConvertToList_Null_ReturnsNull ()
        {
            var result = ((string[]) null).ConvertToList ();

            result.Should ().BeNull ();
        }


        [Fact]
        public void ConvertToArray_ReturnsIArray ()
        {
            // arrange
            var data = new[] { "a", null, "b" };


            // act
            var result = data.Select (x => x).ConvertToArray ();


            // assert
            result.Should ()
                  .BeAssignableTo<string[]> ()
                  .And
                  .NotBeSameAs (data)
                  .And
                  .Equal ("a", null, "b");
        }


        [Fact]
        public void ConvertToArray_Null_ReturnsNull ()
        {
            var result = ((string[]) null).ConvertToArray ();

            result.Should ().BeNull ();
        }


        [Fact]
        public void ConvertToReadOnlyList_ReturnsIReadOnlyList ()
        {
            // arrange
            var data = new[] { "a", null, "b" };


            // act
            var result = data.Select (x => x).ConvertToReadOnlyList ();


            // assert
            result.Should ()
                  .BeAssignableTo<IReadOnlyList<string>> ()
                  .And
                  .NotBeSameAs (data)
                  .And
                  .Equal ("a", null, "b");
        }


        [Fact]
        public void ConvertToReadOnlyList_Null_ReturnsNull ()
        {
            var result = ((string[]) null).ConvertToReadOnlyList ();

            result.Should ().BeNull ();
        }


        [Fact]
        public void ConvertToReadOnlyCollection_ReturnsIReadOnlyCollection ()
        {
            // arrange
            var data = new[] { "a", null, "b" };


            // act
            var result = data.Select (x => x).ConvertToReadOnlyCollection ();


            // assert
            result.Should ()
                  .BeAssignableTo<IReadOnlyCollection<string>> ()
                  .And
                  .NotBeSameAs (data)
                  .And
                  .Equal ("a", null, "b");
        }


        [Fact]
        public void ConvertToReadOnlyCollection_Null_ReturnsNull ()
        {
            var result = ((string[]) null).ConvertToReadOnlyCollection ();

            result.Should ().BeNull ();
        }


        [Fact]
        public void IsNullOrEmpty_Empty_ReturnsTrue ()
        {
            var result = new string[0].IsNullOrEmpty ();

            result.Should ().BeTrue ();
        }


        [Fact]
        public void IsNullOrEmpty_NotEmpty_ReturnsFalse ()
        {
            var result = new[] { "a" }.IsNullOrEmpty ();

            result.Should ().BeFalse ();
        }


        [Fact]
        public void IsNullOrEmpty_Null_ReturnsTrue ()
        {
            var result = ((IEnumerable<string>) null).IsNullOrEmpty ();

            result.Should ().BeTrue ();
        }


        [Fact]
        public void IsNullOrEmpty_ShouldEnumerateOnce ()
        {
            // arrange
            var data = new TestEnumerable<string> (new[] { "a", "b", "c" });


            // act
            var result = data.Enumerate ().IsNullOrEmpty ();


            // assert
            result.Should ().BeFalse ();
            data.EnumeratedCount.Should ().Be (1);
        }


        [Fact]
        public void FirstOrNull_ShouldEnumerateOnce ()
        {
            // arrange
            var data = new TestEnumerable<int> (new[] { 1, 2, 3 });


            // act
            var result = data.Enumerate ().FirstOrNull ();


            // assert
            result.Should ().Be (1);
            data.EnumeratedCount.Should ().Be (1);
        }


        [Fact]
        public void FirstOrNull_Null_ReturnsNull ()
        {
            var result = ((int[]) null).FirstOrNull ();

            result.Should ().NotHaveValue ();
        }


        [Fact]
        public void FirstOrNull_Empty_ReturnsNull ()
        {
            var result = new int[0].FirstOrNull ();

            result.Should ().NotHaveValue ();
        }


        [Fact]
        public void SortById_ReturnsCorrectlySorted ()
        {
            // arrange
            var ids = new[] { 1, 2, 3 };
            var items = new[] { new { id = 2 }, new { id = 1 }, new { id = 3 } };


            // act
            var result = items.SortById (ids, x => x.id);


            // assert
            result.Should().BeEquivalentTo (new[]
                                         {
                                             new { id = 1 },
                                             new { id = 2 },
                                             new { id = 3 }
                                         });
        }


        private class TestDataObject
        {
            public string Name { get; set; }
        }


        [Fact]
        public void DistinctBy_ShouldReturnUniqueWithTheSameOrder ()
        {
            // arrange
            var data = new[]
                       {
                           new TestDataObject { Name = "b" },
                           new TestDataObject { Name = "a" },
                           new TestDataObject { Name = "B" }
                       };


            // act
            var result = data.DistinctBy (x => x.Name, StringComparer.OrdinalIgnoreCase);


            // assert
            result.Select (x => x.Name).Should ().Equal ("b", "a");
        }


        [Fact]
        public void IndexOf_Found_ReturnsCorrectIndex ()
        {
            // arrange
            var data = new[]
                       {
                           new TestDataObject { Name = "a" },
                           new TestDataObject { Name = "b" },
                           new TestDataObject { Name = "c" }
                       };


            // act
            var result = data.IndexOf (x => x.Name == "b");


            // assert
            result.Should ().Be (1);
        }


        [Fact]
        public void IndexOf_NotFound_ReturnsMinusOne ()
        {
            // arrange
            var data = new[]
                       {
                           new TestDataObject { Name = "a" },
                           new TestDataObject { Name = "b" }
                       };


            // act
            var result = data.IndexOf (x => x.Name == "c");


            // assert
            result.Should ().Be (-1);
        }


        [Fact]
        public void SplitToChunks_Null_ReturnsEmpty ()
        {
            // arrange
            var data = (int[]) null;


            // act
            var result = data.SplitToChunks (10);


            // assert
            result.Should ().BeEmpty ();
        }


        [Fact]
        public void SplitToChunks_Empty_ReturnsEmpty ()
        {
            // arrange
            var data = new int[0];


            // act
            var result = data.SplitToChunks (10);


            // assert
            result.Should ().BeEmpty ();
        }


        [Fact]
        public void SplitToChunks_EqualNumberOfItemsToChunkSize_ReturnsOneChunkWithAllItems ()
        {
            // arrange
            var data = new[] { 1, 2, 3 };


            // act
            var result = data.SplitToChunks (3);


            // assert
            result.Should().BeEquivalentTo (new[] { data });
        }


        [Fact]
        public void SplitToChunks_LessNumberOfItemsThanChunkSize_ReturnsOneChunkWithAllItems ()
        {
            // arrange
            var data = new[] { 1, 2, 3 };


            // act
            var result = data.SplitToChunks (4);


            // assert
            result.Should().BeEquivalentTo (new[] { data });
        }


        [Fact]
        public void SplitToChunks_MoreItemsThanChunkSize_ReturnsChunks ()
        {
            // arrange
            var data = new[] { 1, 2, 3 };


            // act
            var result = data.SplitToChunks (2);


            // assert
            result.Should().BeEquivalentTo (new[] { new[] { 1, 2 }, new[] { 3 } });
        }


        [Fact]
        public void AddRange_Null_AddsNothing ()
        {
            // arrange
            ICollection<int> data = new List<int> { 1, 2, 3 };


            // act
            data.AddRange (null);


            // assert
            data.Should().BeEquivalentTo (new[] { 1, 2, 3 });
        }


        [Fact]
        public void AddRange_Empty_AddsNothing ()
        {
            // arrange
            ICollection<int> data = new List<int> { 1, 2, 3 };


            // act
            data.AddRange (new int[0]);


            // assert
            data.Should().BeEquivalentTo (new[] { 1, 2, 3 });
        }


        [Fact]
        public void AddRange_ToList_AddsItems ()
        {
            // arrange
            ICollection<int> data = new List<int> { 1, 2, 3 };


            // act
            data.AddRange (new[] { 4, 5, 6 });


            // assert
            data.Should().BeEquivalentTo (new[] { 1, 2, 3, 4, 5, 6 });
        }


        [Fact]
        public void AddRange_ToCollection_AddsItems ()
        {
            // arrange
            var data = new Collection<int> { 1, 2, 3 };


            // act
            data.AddRange (new[] { 4, 5, 6 });


            // assert
            data.Should().BeEquivalentTo (new[] { 1, 2, 3, 4, 5, 6 });
        }


        [Theory]
        [InlineData (null, null, "", "", "", "")]
        [InlineData ("", "", "", "", "", "")]
        [InlineData (null, "abc", "", "", "", "abc")]
        [InlineData ("", "abc", "", "", "", "abc")]
        [InlineData ("abc", null, "abc", "", "", "")]
        [InlineData ("abc", "", "abc", "", "", "")]
        [InlineData ("abc", "abc", "", "abc", "abc", "")]
        [InlineData ("abc", "ab", "c", "ab", "ab", "")]
        [InlineData ("ab", "abc", "", "ab", "ab", "c")]
        [InlineData ("abd", "abc", "d", "ab", "ab", "c")]
        [InlineData ("abda", "abcb", "d", "aba", "abb", "c")]
        public void CompareTo_Theory_Comply (string source,
                                             string desination,
                                             string expectedOnlyInSource,
                                             string expectedSourceInBoth,
                                             string expectedDestinationInBoth,
                                             string expectedOnlyInDestination)
        {
            // act
#pragma warning disable 618
            var result = source.CompareTo (desination, (a, b) => a == b);
#pragma warning restore 618


            // assert
            result.Should().BeEquivalentTo
                (new CollectionComparisonResult<char>
                 {
                     OnlyInSource = expectedOnlyInSource.ToCharArray (),
                     SourceInBoth = expectedSourceInBoth.ToCharArray (),
                     DestinationInBoth = expectedDestinationInBoth.ToCharArray (),
                     OnlyInDestination = expectedOnlyInDestination.ToCharArray ()
                 });
        }


        [Theory]
        [InlineData (null, null, "", "", "", "")]
        [InlineData ("", "", "", "", "", "")]
        [InlineData (null, "abc", "", "", "", "abc")]
        [InlineData ("", "abc", "", "", "", "abc")]
        [InlineData ("abc", null, "abc", "", "", "")]
        [InlineData ("abc", "", "abc", "", "", "")]
        [InlineData ("abc", "abc", "", "abc", "abc", "")]
        [InlineData ("abc", "ab", "c", "ab", "ab", "")]
        [InlineData ("ab", "abc", "", "ab", "ab", "c")]
        [InlineData ("abd", "abc", "d", "ab", "ab", "c")]
        [InlineData ("abda", "abcb", "d", "aba", "abb", "c")]
        public void CompareTo_ByKey_Theory_Comply (string source,
                                                   string desination,
                                                   string expectedOnlyInSource,
                                                   string expectedSourceInBoth,
                                                   string expectedDestinationInBoth,
                                                   string expectedOnlyInDestination)
        {
            // act
            var result = source.CompareTo (desination, x => x);


            // assert
            result.Should().BeEquivalentTo
                (new CollectionComparisonResult<char>
                 {
                     OnlyInSource = expectedOnlyInSource.ToCharArray (),
                     SourceInBoth = expectedSourceInBoth.ToCharArray (),
                     DestinationInBoth = expectedDestinationInBoth.ToCharArray (),
                     OnlyInDestination = expectedOnlyInDestination.ToCharArray ()
                 });
        }


        [Theory]
        [InlineData (null, null, "", "", "")]
        [InlineData ("abc", null, "abc", "", "")]
        [InlineData (null, "abc", "", "", "abc")]
        [InlineData ("abc", "abc", "", "a-a,b-b,c-c", "")]
        [InlineData ("abcd", "abce", "d", "a-a,b-b,c-c", "e")]
        public void MergeInto_Theory_Comply (string source,
                                             string desination,
                                             string expectedInserts,
                                             string expectedUpdates,
                                             string expectedDeletes)
        {
            // arrange
            var inserts = new List<char> ();
            var updates = new List<string> ();
            var deletes = new List<char> ();

            // act
#pragma warning disable 618
            source.MergeInto (existedItems: desination,
                              compare: (a, b) => a == b,
                              insert: x => inserts.Add (x),
                              update: (s, d) => updates.Add (s + "-" + d),
                              delete: x => deletes.Add (x));
#pragma warning restore 618

            // assert
            inserts.Should ().Equal (expectedInserts);
            updates.Should ().Equal (expectedUpdates.Split (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            deletes.Should ().Equal (expectedDeletes);
        }


        [Theory]
        [InlineData (null, null, "", "", "")]
        [InlineData ("abc", null, "abc", "", "")]
        [InlineData (null, "abc", "", "", "abc")]
        [InlineData ("abc", "abc", "", "a-a,b-b,c-c", "")]
        [InlineData ("abcd", "abce", "d", "a-a,b-b,c-c", "e")]
        public void MergeInto_ByKey_Theory_Comply (string source,
                                                   string desination,
                                                   string expectedInserts,
                                                   string expectedUpdates,
                                                   string expectedDeletes)
        {
            // arrange
            var inserts = new List<char> ();
            var updates = new List<string> ();
            var deletes = new List<char> ();

            // act
            source.MergeInto (existedItems: desination,
                              key: x => x,
                              insert: x => inserts.Add (x),
                              update: (s, d) => updates.Add (s + "-" + d),
                              delete: x => deletes.Add (x));


            // assert
            inserts.Should ().Equal (expectedInserts);
            updates.Should ().Equal (expectedUpdates.Split (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            deletes.Should ().Equal (expectedDeletes);
        }
    }
}