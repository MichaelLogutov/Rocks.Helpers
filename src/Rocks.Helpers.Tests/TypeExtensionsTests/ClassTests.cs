using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rocks.Helpers.Tests.TypeExtensionsTests
{
    [TestClass]
    public class ClassTests
    {
        private class ClassA<T>
        {
        }

        private interface IInterfaceA<T>
        {
        }

        private interface IInterfaceB : IInterfaceA<int>
        {
        }

        private class ClassB<T, T2> : ClassA<T>, IInterfaceA<T>
        {
        }

        private class ClassC<T> : IInterfaceB
        {
        }

        private class ClassD
        {
        }

        private class ClassE : ClassD
        {
        }

        private class NotInheritedClass
        {
        }


        [TestMethod]
        public void DoesNotInherits_ReturnsFalse ()
        {
            // arrange
            var sut = typeof (NotInheritedClass);


            // act
            var result = sut.Implements (typeof (IInterfaceA<>));


            // assert
            result.Should ().BeFalse ();
        }


        [TestMethod]
        public void InheritedOpenGenericInterface_ReturnsTrue ()
        {
            // arrange
            var sut = typeof (ClassB<int, string>);


            // act
            var result = sut.Implements (typeof (IInterfaceA<>));


            // assert
            result.Should ().BeTrue ();
        }


        [TestMethod]
        public void IndirectlyInheritedOpenGenericInterface_ReturnsTrue ()
        {
            // arrange
            var sut = typeof (ClassC<int>);


            // act
            var result = sut.Implements (typeof (IInterfaceA<>));


            // assert
            result.Should ().BeTrue ();
        }


        [TestMethod]
        public void InheritedOpenGenericClass_ReturnsTrue ()
        {
            // arrange
            var sut = typeof (ClassB<int, string>);


            // act
            var result = sut.Implements (typeof (ClassA<>));


            // assert
            result.Should ().BeTrue ();
        }


        [TestMethod]
        public void Self_ReturnsTrue ()
        {
            // arrange
            var sut = typeof (ClassD);


            // act
            var result = sut.Implements (typeof (ClassD));


            // assert
            result.Should ().BeTrue ();
        }


        [TestMethod]
        public void DirectlyInheritsClass_ReturnsTrue ()
        {
            // arrange
            var sut = typeof (ClassE);


            // act
            var result = sut.Implements (typeof (ClassD));


            // assert
            result.Should ().BeTrue ();
        }
    }
}