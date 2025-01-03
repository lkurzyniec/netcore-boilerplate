using System.Diagnostics.CodeAnalysis;
using HappyCode.NetCoreBoilerplate.Api.Infrastructure.Logging;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;

namespace HappyCode.NetCoreBoilerplate.Api.Infrastructure.Configurations
{
    [ExcludeFromCodeCoverage]
    public static class SerilogConfigurator
    {
        public static Logger CreateLogger()
        {
            var configuration = LoadAppConfiguration();
            return new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.With(new VersionEnricher(new ()))
                .CreateLogger();
        }

        private static IConfigurationRoot LoadAppConfiguration()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .Build();
        }
    }
}
