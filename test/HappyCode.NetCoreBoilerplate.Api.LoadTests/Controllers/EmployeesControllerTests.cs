using Xunit;

namespace HappyCode.NetCoreBoilerplate.Api.LoadTests.Controllers
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
    }
}
