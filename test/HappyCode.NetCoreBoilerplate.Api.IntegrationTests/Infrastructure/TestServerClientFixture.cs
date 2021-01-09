using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace HappyCode.NetCoreBoilerplate.Api.IntegrationTests.Infrastructure
{
    public class TestServerClientFixture
    {
        public HttpClient Client { get; }

        public TestServerClientFixture()
        {
            var host = new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseEnvironment("Test")
                        .UseStartup<TestStartup>()
                        .UseTestServer();
                })
                .Start();

            Client = host.GetTestClient();
        }
    }

    [CollectionDefinition(nameof(TestServerClientCollection))]
    public class TestServerClientCollection : ICollectionFixture<TestServerClientFixture>
    {
    }
}
