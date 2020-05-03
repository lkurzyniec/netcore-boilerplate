using System.Diagnostics;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace HappyCode.NetCoreBoilerplate.Api.LoadTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var assembly = typeof(Program).Assembly;
            var config = Debugger.IsAttached ? (IConfig)new DebugInProcessConfig() : new Config();

            _ = args.Length > 0
                ? BenchmarkSwitcher.FromAssembly(assembly).Run(args, config)
                : BenchmarkRunner.Run(assembly, config);
        }
    }

    public class Config : ManualConfig
    {
        public Config()
        {
            Add(DefaultConfig.Instance);

            AddColumn(StatisticColumn.Max);

            Options |= ConfigOptions.JoinSummary;
        }
    }
}
