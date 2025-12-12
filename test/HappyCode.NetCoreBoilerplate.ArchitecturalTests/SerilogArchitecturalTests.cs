using HappyCode.NetCoreBoilerplate.Api;
using NetArchTest.Rules;
using Xunit;

namespace HappyCode.NetCoreBoilerplate.ArchitecturalTests;

public class SerilogArchitecturalTests
{
    private static readonly Types _apiTypes = Types.InAssembly(typeof(Startup).Assembly);

    [Fact]
    public void Only_MS_ILogger_is_allowed()
    {
        var result = _apiTypes
            .That()
            .DoNotHaveName(nameof(Program)) // except Program which is an entrypoint
            .Should()
            .NotHaveDependencyOn($"Serilog.{nameof(Serilog.ILogger)}")
            .GetResult();

        Assert.True(result.IsSuccessful, $"Failing Types: {string.Join("; ", result.FailingTypeNames ?? Array.Empty<string>())}");
    }
}
