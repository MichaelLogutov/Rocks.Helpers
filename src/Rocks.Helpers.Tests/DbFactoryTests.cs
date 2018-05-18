using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using FluentAssertions;
using Xunit;


namespace Rocks.Helpers.Tests
{
    [Collection("NoParallel")]
    public class DbFactoryTests
    {
        private sealed class TestDbProviderFactory : DbProviderFactory
        {
        }

        private sealed class TestDbProviderFactory2 : DbProviderFactory
        {
        }


        [Fact]
        public void Get_BuiltInSqlClientFactory_ShoulBeCorrectlyType()
        {
            // arrange


            // act
            var result = DbFactory.Get("System.Data.SqlClient");


            // assert
            result.Should().BeOfType<SqlClientFactory>();
        }


        [Fact]
        public void Set_ShoulBeCorrectlyType()
        {
            // arrange


            // act
            DbFactory.Set("aaa", new TestDbProviderFactory());
            var result = DbFactory.Get("aaa");


            // assert
            result.Should().BeOfType<TestDbProviderFactory>();
        }


        [Fact]
        public void SetConstructInstanceInterceptor_SetNewInstance_ShoulBeSetOnceTime()
        {
            // arrange
            var calls = new List<string>();

            DbFactory.Set("bbb", new TestDbProviderFactory());
            DbFactory.SetConstructInstanceInterceptor(
                instance =>
                {
                    if (!(instance is TestDbProviderFactory))
                        return instance;

                    calls.Add("ExecuteConstructInstanceInterceptor");

                    var new_instance = new TestDbProviderFactory2();
                    DbFactory.Set("bbb", new_instance);

                    return new_instance;
                });


            // act
            var result_1 = DbFactory.Get("bbb");
            var result_2 = DbFactory.Get("bbb");


            // assert
            result_1.Should().BeOfType<TestDbProviderFactory2>();
            result_2.Should().BeOfType<TestDbProviderFactory2>();
            calls.Should().BeEquivalentTo("ExecuteConstructInstanceInterceptor");
        }
    }
}