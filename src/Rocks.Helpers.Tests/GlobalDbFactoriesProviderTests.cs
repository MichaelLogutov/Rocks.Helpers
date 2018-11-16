using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using FluentAssertions;
using Xunit;


namespace Rocks.Helpers.Tests
{
    [Collection("NoParallel")]
    public class GlobalDbFactoriesProviderTests
    {
        private sealed class TestDbProviderFactory : DbProviderFactory
        {
        }

        private sealed class TestDbProviderFactory2 : DbProviderFactory
        {
        }


        [Fact]
        public void Get_BuiltInSqlClientFactory_ShouldBeCorrectlyType()
        {
            // arrange


            // act
            var result = GlobalDbFactoriesProvider.Get("System.Data.SqlClient");


            // assert
            result.Should().BeOfType<SqlClientFactory>();
        }


        [Fact]
        public void Set_ShouldBeCorrectlyType()
        {
            // arrange


            // act
            GlobalDbFactoriesProvider.Set("aaa", new TestDbProviderFactory());
            var result = GlobalDbFactoriesProvider.Get("aaa");


            // assert
            result.Should().BeOfType<TestDbProviderFactory>();
        }


        [Fact]
        public void SetConstructInstanceInterceptor_SetNewInstance_ShouldBeSetOnceTime()
        {
            // arrange
            var calls = new List<string>();

            GlobalDbFactoriesProvider.Set("bbb", new TestDbProviderFactory());
            GlobalDbFactoriesProvider.SetConstructInstanceInterceptor(
                instance =>
                {
                    if (!(instance is TestDbProviderFactory))
                        return instance;

                    calls.Add("ExecuteConstructInstanceInterceptor");

                    var new_instance = new TestDbProviderFactory2();
                    GlobalDbFactoriesProvider.Set("bbb", new_instance);

                    return new_instance;
                });


            // act
            var result_1 = GlobalDbFactoriesProvider.Get("bbb");
            var result_2 = GlobalDbFactoriesProvider.Get("bbb");


            // assert
            result_1.Should().BeOfType<TestDbProviderFactory2>();
            result_2.Should().BeOfType<TestDbProviderFactory2>();
            calls.Should().BeEquivalentTo("ExecuteConstructInstanceInterceptor");
        }
    }
}