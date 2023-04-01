using FluentAssertions;
using HappyCode.NetCoreBoilerplate.Core;
using NetArchTest.Rules;
using Xunit;

namespace HappyCode.NetCoreBoilerplate.ArchitectureTests
{
    public class CoreArchitectureTests
    {
        private static readonly Types _coreTypes = Types.InAssembly(typeof(CarsContext).Assembly);

        [Fact]
        public void Core_should_not_use_AspNetCore()
        {
            var result = _coreTypes
                .ShouldNot()
                .HaveDependencyOn("Microsoft.AspNetCore")
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }
    }
}
