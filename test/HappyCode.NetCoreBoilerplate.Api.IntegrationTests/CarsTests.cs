using System.Net;
using AwesomeAssertions;
using HappyCode.NetCoreBoilerplate.Api.IntegrationTests.Infrastructure;
using HappyCode.NetCoreBoilerplate.Core.Dtos;

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
            //when
            var result = await _client.GetAsync($"api/cars", TestContext.Current.CancellationToken);

            //then
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            var cars = await result.Content.ReadFromJsonAsync<List<CarDto>>(TestContext.Current.CancellationToken);
            cars.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public Task Get_should_return_expected_json()
        {
            //when
            var result = _client.GetAsync($"api/cars", TestContext.Current.CancellationToken);

            //then
            return Verify(result);
        }
    }
}
