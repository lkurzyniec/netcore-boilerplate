using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HappyCode.NetCoreBoilerplate.Api.IntegrationTests.Infrastructure;
using HappyCode.NetCoreBoilerplate.Core.Dtos;
using Shouldly;
using Xunit;

namespace HappyCode.NetCoreBoilerplate.Api.IntegrationTests
{
    [Collection(nameof(TestServerClientCollection))]
    public class CarsTests
    {
        private readonly HttpClient _client;

        public CarsTests(TestServerClientFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task Get_should_return_Ok_with_results()
        {
            var result = await _client.GetAsync($"api/cars");

            result.StatusCode.ShouldBe(HttpStatusCode.OK);
            var cars = await result.Content.ReadAsJsonAsync<List<CarDto>>();
            cars.Count.ShouldBeGreaterThan(0);
        }
    }
}
