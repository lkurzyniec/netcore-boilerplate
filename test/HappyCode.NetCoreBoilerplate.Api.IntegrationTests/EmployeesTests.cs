using System;
using System.Linq;
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
            var result = await _client.GetAsync("api/employees/123456");

            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Get_should_return_Ok_with_expected_result()
        {
            var result = await _client.GetAsync("api/employees/1");

            result.EnsureSuccessStatusCode();
            var emp = await result.Content.ReadAsJsonAsync<EmployeeDto>();
            emp.LastName.Should().Be("Anderson");
        }

        [Fact]
        public async Task Delete_should_return_NoContent_when_delete_employee()
        {
            var result = await _client.DeleteAsync("api/employees/99");

            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Delete_should_return_NotFound_when_no_employee()
        {
            var result = await _client.DeleteAsync("api/employees/98765");

            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Put_should_return_NotFound_when_no_employee()
        {
            var request = new EmployeePutDto { LastName = "Smith" };
            var result = await _client.PutAsync("api/employees/98765", request.ToStringContent());

            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Put_should_return_Ok_with_result_when_successfully_updated()
        {
            var request = new EmployeePutDto { LastName = "Smith" };
            var result = await _client.PutAsync("api/employees/2", request.ToStringContent());

            result.EnsureSuccessStatusCode();
            var emp = await result.Content.ReadAsJsonAsync<EmployeeDto>();
            emp.LastName.Should().Be("Smith");
        }

        [Fact]
        public async Task Post_should_return_Created_with_result_and_link_when_successfully_created()
        {
            var request = new EmployeePostDto { FirstName = "Joann", LastName = "Richardson", Gender = "F", BirthDate = new DateTime(2003, 5, 1) };
            var result = await _client.PostAsync("api/employees/", request.ToStringContent());

            result.EnsureSuccessStatusCode();
            var emp = await result.Content.ReadAsJsonAsync<EmployeeDto>();
            emp.LastName.Should().Be("Richardson");

            result.Headers.Location.ToString().Should().Contain("api/employees/100");

            result.Headers.TryGetValues("x-date-created", out _).Should().BeTrue();
        }
    }
}
