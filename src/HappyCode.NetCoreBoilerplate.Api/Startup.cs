using System.Diagnostics.CodeAnalysis;
using HappyCode.NetCoreBoilerplate.Api.BackgroundServices;
using HappyCode.NetCoreBoilerplate.Api.Infrastructure.Configurations;
using HappyCode.NetCoreBoilerplate.Api.Infrastructure.Filters;
using HappyCode.NetCoreBoilerplate.Api.Infrastructure.Middlewares;
using HappyCode.NetCoreBoilerplate.Api.Infrastructure.OpenApi;
using HappyCode.NetCoreBoilerplate.BooksModule;
using HappyCode.NetCoreBoilerplate.Core;
using HappyCode.NetCoreBoilerplate.Core.Providers;
using HappyCode.NetCoreBoilerplate.Core.Registrations;
using HappyCode.NetCoreBoilerplate.Core.Settings;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement;
using Serilog;
using Scalar.AspNetCore;

namespace HappyCode.NetCoreBoilerplate.Api
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddSerilog();
            services
                .AddSingleton<ExceptionMiddleware>()
                .AddSingleton<BadHttpRequestExceptionMiddleware>();

            services
                .AddHttpContextAccessor()
                .AddRouting(options => options.LowercaseUrls = true);

            services.AddMvcCore(options =>
                {
                    options.Filters.Add<HttpGlobalExceptionFilter>();
                    options.Filters.Add<ApiKeyAuthorizationFilter>();
                })
                .AddApiExplorer()
                .AddDataAnnotations();

            //there is a difference between AddDbContext() and AddDbContextPool(), more info https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-2.0#dbcontext-pooling and https://stackoverflow.com/questions/48443567/adddbcontext-or-adddbcontextpool
            services.AddDbContext<EmployeesContext>(options => options.UseMySQL(_configuration.GetConnectionString("MySqlDb")), contextLifetime: ServiceLifetime.Transient, optionsLifetime: ServiceLifetime.Singleton);
            services.AddDbContextPool<CarsContext>(options => options.UseSqlServer(_configuration.GetConnectionString("MsSqlDb")), poolSize: 10);

            services.Configure<ApiKeySettings>(_configuration.GetSection("ApiKey"));
            services.AddOpenApi(_configuration);

            services.Configure<PingWebsiteSettings>(_configuration.GetSection("PingWebsite"));
            services.AddHttpClient(nameof(PingWebsiteBackgroundService));
            services.AddHostedService<PingWebsiteBackgroundService>();
            services.AddSingleton(x => x.GetServices<IHostedService>().OfType<IPingService>().Single());

            services.AddCoreComponents();
            services.AddBooksModule(_configuration);

            services.AddFeatureManagement();

            var healthChecksBuilder = services.AddHealthChecks()
                .AddBooksModule(_configuration);
            if (_configuration.GetValue<bool>($"FeatureManagement:{FeatureFlags.DockerCompose}"))
            {
                healthChecksBuilder
                    .AddMySql(_configuration.GetConnectionString("MySqlDb"), tags: ["ready"])
                    .AddSqlServer(_configuration.GetConnectionString("MsSqlDb"), tags: ["ready"]);
            }
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<BadHttpRequestExceptionMiddleware>();
            app.UseMiddlewareForFeature<ConnectionInfoMiddleware>(FeatureFlags.ConnectionInfo.ToString());

            app.UseSerilogRequestLogging();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/healthz/live", new HealthCheckOptions
                {
                    Predicate = _ => false,
                }).ShortCircuit();
                endpoints.MapHealthChecks("/healthz/ready", new HealthCheckOptions
                {
                    Predicate = healthCheck => healthCheck.Tags.Contains("ready"),
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                }).ShortCircuit();

                endpoints.MapGet("/version", (VersionProvider provider) => provider.VersionEntries)
                    .ExcludeFromDescription();

                endpoints.MapControllers();
                endpoints.MapBooksModule();

                endpoints.MapOpenApi()
                    .CacheOutput();

                endpoints.MapScalarApiReference("api-doc");
            });

            app.InitBooksModule();
        }
    }
}
