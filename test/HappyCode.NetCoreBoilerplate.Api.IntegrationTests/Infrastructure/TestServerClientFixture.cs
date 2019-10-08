using System;
using System.Net.Http;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace HappyCode.NetCoreBoilerplate.Api.IntegrationTests.Infrastructure
{
    public class TestServerClientFixture
    {
        public HttpClient Client { get; }

        public TestServerClientFixture()
        {
            var server = new TestServer(WebHost.CreateDefaultBuilder()
                .UseEnvironment("Test")
                .ConfigureServices(services => services.AddAutofac())
                .UseStartup<TestStartup>())
                {
                    BaseAddress = new Uri("http://localhost:5000")
                };

            Client = server.CreateClient();
        }
    }

    [CollectionDefinition(nameof(TestServerClientCollection))]
    public class TestServerClientCollection : ICollectionFixture<TestServerClientFixture>
    {
    }
}
