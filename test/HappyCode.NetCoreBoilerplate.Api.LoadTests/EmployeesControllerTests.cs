using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using HappyCode.NetCoreBoilerplate.Api.LoadTests.Extensions;
using NBomber.Http.CSharp;
using Xunit;

namespace HappyCode.NetCoreBoilerplate.Api.LoadTests
{
    public class EmployeesControllerTests : LoadTestsBase
    {
        protected override string ResourceUrl => "api/employees";

        [Fact]
        public void GetAll_load_test()
        {
            ExecuteLoadTest(action: "/");
        }


        [Fact]
        public void Get_load_test()
        {
            ExecuteLoadTest(action: "/1");
        }

        [Fact]
        public void Post_with_Get_load_test()
        {
            const string request = @"{
                                        ""firstName"": ""load"",
                                        ""lastName"": ""test"",
                                        ""gender"": ""M"",
                                        ""birthDate"": ""1980-11-11""
                                    }";

            var post = CreateStep("/", "POST", request.ToStringContent());

            var get = HttpStep.Create("get emp", async ctx =>
            {
                var response = ctx.GetPreviousStepResponse<HttpResponseMessage>();
                var content = await response.Content.ReadAsStringAsync();
                var emp = content.Deserialize<EmployeeDto>();
                Trace.WriteLine($"The previous step created employee with Id = {emp.Id}");

                var getUrl = response.Headers.Location;

                return Http.CreateRequest("GET", getUrl.ToString())
                    .WithHeader("Authorization", "ApiKey ABC-xyz")
                    .WithCheck(response => Task.FromResult(response.IsSuccessStatusCode));
            });

            ExecuteLoadTests(post, get);
        }

        private class EmployeeDto
        {
            public int Id { get; set; }
        }
    }
}
