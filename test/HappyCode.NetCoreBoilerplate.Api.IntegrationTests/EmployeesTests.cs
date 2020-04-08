using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using HappyCode.NetCoreBoilerplate.Api.IntegrationTests.Infrastructure;
using HappyCode.NetCoreBoilerplate.Core.Dtos;
using Xunit;

namespace HappyCode.NetCoreBoilerplate.Api.IntegrationTests
{
    [Collection(nameof(TestServerClientCollection))]
    public class EmployeesTests
    {
        private readonly HttpClient _client;

        public EmployeesTests(TestServerClientFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task Get_should_return_NotFound_when_no_employee()
        {
            var result = await _client.GetAsync($"api/employees/123456");

            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Get_should_return_Ok_with_expected_result()
        {
            var result = await _client.GetAsync($"api/employees/1");

            result.StatusCode.Should().Be(HttpStatusCode.OK);
            var emp = await result.Content.ReadAsJsonAsync<EmployeeDto>();
            emp.LastName.Should().Be("Anderson");
        }

        [Fact]
        public async Task Delete_should_return_NoContent_when_delete_employee()
        {
            var result = await _client.DeleteAsync($"api/employees/99");

            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Delete_should_return_NotFound_when_no_employee()
        {
            var result = await _client.DeleteAsync($"api/employees/98765");

            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
