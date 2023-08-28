using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using HappyCode.NetCoreBoilerplate.BooksModule.IntegrationTests.Infrastructure.DataFeeders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace HappyCode.NetCoreBoilerplate.BooksModule.IntegrationTests.Infrastructure
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
                        .ConfigureServices(services =>
                        {
                            var config = new ConfigurationBuilder()
                                .AddInMemoryCollection(new Dictionary<string, string>(1) { { "ConnectionStrings:SqliteDb", $"Data Source=tests_tempdb_{DateTimeOffset.Now.ToUnixTimeSeconds()}.db" } })
                                .Build();
                            services.AddRouting();
                            services.AddBooksModule(config);
                        })
                        .Configure(app =>
                        {
                            app.UseRouting();
                            app.UseEndpoints(endpoints => endpoints.MapBooksModule());

                            app.InitBooksModule();
                            var db = app.ApplicationServices.GetService<IDbConnection>();
                            BooksDataFeeder.Feed(db);
                        })
                        .UseEnvironment("Test")
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
