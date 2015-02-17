using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rocks.Helpers.Tests.TypeExtensionsTests
{
    [TestClass]
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


        [TestMethod]
        public void Interface_DoesNotInherits_ReturnsFalse ()
        {
            // arrange
            var sut = typeof (INotInheritedInterface);


            // act
            var result = sut.Implements (typeof (IInterfaceA<>));


            // assert
            result.Should ().BeFalse ();
        }


        [TestMethod]
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