using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace HappyCode.NetCoreBoilerplate.Api.LoadTests.Infrastructure
{
    public class HttpClientFixture
    {
        public HttpClient Client { get; }

        public HttpClientFixture()
        {
            string baseUrl = Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "http://localhost:5000/";

            Client = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };

            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Add("Accept", "application/json");
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("ApiKey", "ABC-xyz");
        }
    }
}
