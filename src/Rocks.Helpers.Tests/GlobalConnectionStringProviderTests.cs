using System;
using FluentAssertions;
using Xunit;
#if NETCOREAPP
using Microsoft.Extensions.Configuration;

#endif

namespace Rocks.Helpers.Tests
{
    [Collection("NoParallel")]
    public class GlobalConnectionStringProviderTests
    {
        public GlobalConnectionStringProviderTests()
        {
#if NETCOREAPP
            GlobalConfigurationProvider.Set(() => new ConfigurationBuilder()
                                                  .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                                                  .Build());
#endif
        }


        [Fact]
        public void HasTransform_UsesIt()
        {
            // arrange
            GlobalConnectionStringProvider.CustomProvider = _ => "server=.; database=test; Integrated Security=True";
            GlobalConnectionStringProvider.ConnectionStringTransform = x => x + "; Application Name = abc";


            // act
            var result = GlobalConnectionStringProvider.Get("test");


            // assert
            result.Should().Be("server=.; database=test; Integrated Security=True; Application Name = abc");
        }


        [Fact]
        public void HasTransform_TransformReturnsNull_Throws()
        {
            // arrange
            GlobalConnectionStringProvider.CustomProvider = _ => "server=.; database=test; Integrated Security=True";
            GlobalConnectionStringProvider.ConnectionStringTransform = _ => null;


            // act
            Action act = () => GlobalConnectionStringProvider.Get("test");


            // assert
            act.Should().Throw<InvalidOperationException>();
        }


        [Fact]
        public void HasConnectionStringInConfigurationManager_HasNoCustomProvider_UsesFromConfig()
        {
            // arrange
            GlobalConnectionStringProvider.CustomProvider = _ => null;
            GlobalConnectionStringProvider.ConnectionStringTransform = null;


            // act
            var result = GlobalConnectionStringProvider.Get("TestInAppConfig");


            // assert
            result.Should().Be("test connection string from app.config");
        }


        [Fact]
        public void HasConnectionStringInConfigurationManager_HasCustomProvider_UsesCustomProviderFirst()
        {
            // arrange
            GlobalConnectionStringProvider.CustomProvider = _ => "bbb";
            GlobalConnectionStringProvider.ConnectionStringTransform = null;


            // act
            var result = GlobalConnectionStringProvider.Get("TestInAppConfig");


            // assert
            result.Should().Be("bbb");
        }
    }
}