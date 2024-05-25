using System;
using HappyCode.NetCoreBoilerplate.Core;
using Microsoft.EntityFrameworkCore;
using NetArchTest.Rules;
using Xunit;

namespace HappyCode.NetCoreBoilerplate.ArchitecturalTests
{
    public class CoreArchitecturalTests
    {
        private static readonly Types _coreTypes = Types.InAssembly(typeof(FeatureFlags).Assembly);

        [Fact]
        public void Core_should_not_use_AspNetCore()
        {
            var result = _coreTypes
                .ShouldNot()
                .HaveDependencyOn("Microsoft.AspNetCore")
                .GetResult();

            Assert.True(result.IsSuccessful, $"Failing Types: {string.Join("; ", result.FailingTypeNames ?? Array.Empty<string>())}");
        }

        [Fact]
        public void Repositories_should_not_use_Services()
        {
            var result = _coreTypes
                .That()
                .ResideInNamespace("HappyCode.NetCoreBoilerplate.Core.Repositories")
                .ShouldNot()
                .HaveDependencyOn("HappyCode.NetCoreBoilerplate.Core.Services")
                .GetResult();

            Assert.True(result.IsSuccessful, $"Failing Types: {string.Join("; ", result.FailingTypeNames ?? Array.Empty<string>())}");
        }

        [Fact]
        public void Repositories_should_have_Repository_suffix()
        {
            var result = _coreTypes
                .That()
                .ResideInNamespace("HappyCode.NetCoreBoilerplate.Core.Repositories")
                .And()
                .AreNotAbstract()
                .Should()
                .HaveNameEndingWith("Repository")
                .GetResult();

            Assert.True(result.IsSuccessful, $"Failing Types: {string.Join("; ", result.FailingTypeNames ?? Array.Empty<string>())}");
        }

        [Fact]
        public void Services_should_have_Service_suffix()
        {
            var result = _coreTypes
                .That()
                .ResideInNamespace("HappyCode.NetCoreBoilerplate.Core.Services")
                .And()
                .AreNotAbstract()
                .Should()
                .HaveNameEndingWith("Service")
                .GetResult();

            Assert.True(result.IsSuccessful, $"Failing Types: {string.Join("; ", result.FailingTypeNames ?? Array.Empty<string>())}");
        }

        [Fact]
        public void Dto_should_have_Dto_suffix()
        {
            var result = _coreTypes
                .That()
                .ResideInNamespace("HappyCode.NetCoreBoilerplate.Core.Dtos")
                .Should()
                .HaveNameEndingWith("Dto")
                .GetResult();

            Assert.True(result.IsSuccessful, $"Failing Types: {string.Join("; ", result.FailingTypeNames ?? Array.Empty<string>())}");
        }

        [Fact]
        public void DbContext_should_have_Context_suffix()
        {
            var result = _coreTypes
                .That()
                .Inherit(typeof(DbContext))
                .Should()
                .HaveNameEndingWith("Context")
                .GetResult();

            Assert.True(result.IsSuccessful, $"Failing Types: {string.Join("; ", result.FailingTypeNames ?? Array.Empty<string>())}");
        }
    }
}
