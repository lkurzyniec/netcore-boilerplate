using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using HappyCode.NetCoreBoilerplate.Api.IntegrationTests.Extensions;
using HappyCode.NetCoreBoilerplate.Api.IntegrationTests.Infrastructure;
using HappyCode.NetCoreBoilerplate.Core.Dtos;
using VerifyXunit;
using Xunit;

namespace HappyCode.NetCoreBoilerplate.Api.IntegrationTests
{
    [UsesVerify]
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
            var result = await _client.GetAsync($"api/cars");

            //then
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            var cars = await result.Content.ReadAsJsonAsync<List<CarDto>>();
            cars.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public Task Get_should_return_expected_json()
        {
            //when
            var result = _client.GetAsync($"api/cars");

            //then
            return Verifier.Verify(result);
        }
    }
}
