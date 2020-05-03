using System.Net.Http;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using HappyCode.NetCoreBoilerplate.Api.LoadTests.Infrastructure;

namespace HappyCode.NetCoreBoilerplate.Api.LoadTests
{
    public abstract class BenchmarksBase
    {
        //[Params(5, 50, 100)]
        //public int NumberOfRequests;

        private HttpClient _client;

        [GlobalSetup]
        public void Setup()
        {
            _client = new HttpClientFixture().Client;
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            _client?.Dispose();
        }

        protected async ValueTask ExecuteTest(string url)
        {
            //for (int i = 0; i < NumberOfRequests; i++)
            {
                using var result = await _client.GetAsync(url);
                result.EnsureSuccessStatusCode();
            }
        }
    }
}
