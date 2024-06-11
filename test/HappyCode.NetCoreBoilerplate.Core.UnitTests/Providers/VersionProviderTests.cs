using FluentAssertions;
using HappyCode.NetCoreBoilerplate.Core.Providers;
using Xunit;

namespace HappyCode.NetCoreBoilerplate.Core.UnitTests.Providers;

public class VersionProviderTests
{
    private readonly VersionProvider _provider;

    public VersionProviderTests()
    {
        _provider = new VersionProvider();
    }

    [Fact]
    public void Provided_should_returns_expected_values()
    {
        // act
        var result = _provider.VersionEntries;

        // assert
        result.Should().NotContainKeys("TEST_ENV", "HC_SHA", "HC_VERSION");
        result.Should().ContainKeys("SHA", "VERSION");
        result.Should().HaveCount(2);
    }
}
