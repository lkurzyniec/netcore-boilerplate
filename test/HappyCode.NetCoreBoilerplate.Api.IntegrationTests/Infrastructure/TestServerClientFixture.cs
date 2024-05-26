using Microsoft.AspNetCore.TestHost;

namespace HappyCode.NetCoreBoilerplate.Api.IntegrationTests.Infrastructure
{
    public class TestServerClientFixture
    {
        public HttpClient Client { get; }

        public TestServerClientFixture()
        {
            var host = new HostBuilder()
                .UseEnvironment("Test")
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseConfiguration(LoadAppConfiguration())
                        .UseStartup<TestStartup>()
                        .UseTestServer();
                })
                .Start();

            Client = host.GetTestClient();
        }

        private static IConfigurationRoot LoadAppConfiguration()
        {
            return new ConfigurationBuilder()
                .AddJsonFile($"appsettings.Test.json", optional: false)
                .Build();
        }
    }

    [CollectionDefinition(nameof(TestServerClientCollection))]
    public class TestServerClientCollection : ICollectionFixture<TestServerClientFixture>
    {
    }
}
