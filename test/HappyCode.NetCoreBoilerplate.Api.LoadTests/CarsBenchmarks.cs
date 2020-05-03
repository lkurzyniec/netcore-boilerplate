using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace HappyCode.NetCoreBoilerplate.Api.LoadTests
{
    public class CarsBenchmarks : BenchmarksBase
    {
        [Benchmark]
        public ValueTask GetCarsLoadTest()
        {
            return ExecuteTest("api/cars");
        }
    }
}
