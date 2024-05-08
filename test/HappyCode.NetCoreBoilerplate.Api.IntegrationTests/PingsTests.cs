using FluentAssertions;
using HappyCode.NetCoreBoilerplate.Api.IntegrationTests.Infrastructure;
using HappyCode.NetCoreBoilerplate.Api.IntegrationTests.Infrastructure.Fakes;

namespace HappyCode.NetCoreBoilerplate.Api.IntegrationTests
{
    [Collection(nameof(TestServerClientCollection))]
    public class PingsTests
    {
        private readonly HttpClient _client;

        public PingsTests(TestServerClientFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task Get_website_should_return_Ok_with_result()
        {
            //when
            var result = await _client.GetAsync("api/pings/website");

            //then
            result.EnsureSuccessStatusCode();
            var status = await result.Content.ReadAsStringAsync();
            status.Should().Contain(FakePingService.Result.ToString());
        }

        [Fact]
        public async Task Get_random_should_return_Ok_with_not_empty_result()
        {
            //when
            var result = await _client.GetAsync("api/pings/random");

            //then
            result.EnsureSuccessStatusCode();
            var status = await result.Content.ReadAsStringAsync();
            status.Should().NotBeNullOrEmpty();
        }
    }
}
