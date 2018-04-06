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
        public void Set_BuiltInSqlClientFactory_ShoulBeCorrectlyType()
        {
            // arrange
            
            
            // act
            DbFactory.Set("System.Data.SqlClient", new TestDbProviderFactory());
            var result = DbFactory.Get("System.Data.SqlClient");


            // assert
            result.Should().BeOfType<TestDbProviderFactory>();
        }
        
        [Fact]
        public void SetConstructInstanceInterceptor_SetNewInstance_ShoulBeSetOnceTime()
        {
            // arrange
            var calls = new List<string>();
            
            DbFactory.SetConstructInstanceInterceptor((instance) =>
                                                      {
                                                          if (instance is TestDbProviderFactory)
                                                          {
                                                              return instance;
                                                          }
                                                          
                                                          calls.Add("ExecuteConstructInstanceInterceptor");
                                                          
                                                          var newInstance = new TestDbProviderFactory();
                                                          
                                                          DbFactory.Set(instance.GetType().Namespace, newInstance);
                                                          
                                                          return newInstance;
                                                      });
            
            
            // act
            var result_1 = DbFactory.Get("System.Data.SqlClient");
            var result_2 = DbFactory.Get("System.Data.SqlClient");


            // assert
            result_1.Should().BeOfType<TestDbProviderFactory>();
            result_2.Should().BeOfType<TestDbProviderFactory>();
            calls.ShouldAllBeEquivalentTo(new[]
                                          {
                                              "ExecuteConstructInstanceInterceptor"          
                                          });
        }
    }
}