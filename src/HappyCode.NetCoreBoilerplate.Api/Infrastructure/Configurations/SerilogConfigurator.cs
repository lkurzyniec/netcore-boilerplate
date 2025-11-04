using System.Diagnostics.CodeAnalysis;
using System.Linq;
using HappyCode.NetCoreBoilerplate.Api.Infrastructure.Logging;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Filters;

namespace HappyCode.NetCoreBoilerplate.Api.Infrastructure.Configurations
{
    [ExcludeFromCodeCoverage]
    public static class SerilogConfigurator
    {
        private static readonly IEnumerable<string> _pathsToOmit = ["/healthz", "/api-doc", "/openapi", "/favicon"];

        public static Logger CreateLogger()
        {
            var configuration = LoadAppConfiguration();
            return new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Filter.ByExcluding(Matching.WithProperty("RequestPath", "/"))
                .Filter.ByIncludingOnly(e => e.Properties.TryGetValue("RequestPath", out var path) ? _pathsToOmit.Any(o => path.ToString().Contains(o)) ? e.Level >= LogEventLevel.Warning : true : true)
                .MinimumLevel.Override("Microsoft.AspNetCore.Hosting.Diagnostics", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Routing.EndpointMiddleware", LogEventLevel.Warning)
                .Enrich.With(new VersionEnricher(new()))
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
