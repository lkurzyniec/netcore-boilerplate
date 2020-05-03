using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace HappyCode.NetCoreBoilerplate.Api.LoadTests
{
    public class EmployeesBenchmarks : BenchmarksBase
    {
        [Benchmark]
        public ValueTask GetEmployeesLoadTest()
        {
            return ExecuteTest("api/employees");
        }

        [Benchmark]
        public ValueTask GetEmployeeLoadTest()
        {
            return ExecuteTest("api/employees/1");
        }
    }
}
