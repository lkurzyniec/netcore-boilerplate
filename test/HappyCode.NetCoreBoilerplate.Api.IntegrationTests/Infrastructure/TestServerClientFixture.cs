using Microsoft.AspNetCore.TestHost;

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
