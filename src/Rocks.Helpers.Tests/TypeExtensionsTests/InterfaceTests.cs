using FluentAssertions;
using Xunit;

namespace Rocks.Helpers.Tests.TypeExtensionsTests
{
    public class InterfaceTests
    {
        private interface IInterfaceA<T>
        {
        }

        private interface IInterfaceB : IInterfaceA<int>
        {
        }


        private interface INotInheritedInterface
        {
        }


        [Fact]
        public void Interface_DoesNotInherits_ReturnsFalse ()
        {
            // arrange
            var sut = typeof (INotInheritedInterface);


            // act
            var result = sut.Implements (typeof (IInterfaceA<>));


            // assert
            result.Should ().BeFalse ();
        }


        [Fact]
        public void Interface_InheritedOpenGenericInterface_ReturnsTrue ()
        {
            // arrange
            var sut = typeof (IInterfaceB);

            // act
            var result = sut.Implements (typeof (IInterfaceA<>));


            // assert
            result.Should ().BeTrue ();
        }
    }
}


