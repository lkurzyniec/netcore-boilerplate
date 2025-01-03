using System.Linq;
using HappyCode.NetCoreBoilerplate.Api.BackgroundServices;
using HappyCode.NetCoreBoilerplate.Api.Infrastructure.Configurations;
using HappyCode.NetCoreBoilerplate.Api.Infrastructure.Filters;
using HappyCode.NetCoreBoilerplate.Api.Infrastructure.Middlewares;
using HappyCode.NetCoreBoilerplate.Api.Infrastructure.Registrations;
using HappyCode.NetCoreBoilerplate.BooksModule;
using HappyCode.NetCoreBoilerplate.Core;
using HappyCode.NetCoreBoilerplate.Core.Providers;
using HappyCode.NetCoreBoilerplate.Core.Registrations;
using HappyCode.NetCoreBoilerplate.Core.Settings;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace HappyCode.NetCoreBoilerplate.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services
                .AddHttpContextAccessor()
                .AddRouting(options => options.LowercaseUrls = true);

            services.AddMvcCore(options =>
                {
                    options.Filters.Add<HttpGlobalExceptionFilter>();
                    options.Filters.Add<ValidateModelStateFilter>();
                    options.Filters.Add<ApiKeyAuthorizationFilter>();
                })
                .AddApiExplorer()
                .AddDataAnnotations();

            //there is a difference between AddDbContext() and AddDbContextPool(), more info https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-2.0#dbcontext-pooling and https://stackoverflow.com/questions/48443567/adddbcontext-or-adddbcontextpool
            services.AddDbContext<EmployeesContext>(options => options.UseMySql(_configuration.GetConnectionString("MySqlDb"), ServerVersion.Parse("8.0")), contextLifetime: ServiceLifetime.Transient, optionsLifetime: ServiceLifetime.Singleton);
            services.AddDbContextPool<CarsContext>(options => options.UseSqlServer(_configuration.GetConnectionString("MsSqlDb")), poolSize: 10);

            services.Configure<ApiKeySettings>(_configuration.GetSection("ApiKey"));
            services.AddSwagger(_configuration);

            services.Configure<PingWebsiteSettings>(_configuration.GetSection("PingWebsite"));
            services.AddHttpClient(nameof(PingWebsiteBackgroundService));
            services.AddHostedService<PingWebsiteBackgroundService>();
            services.AddSingleton(x => x.GetServices<IHostedService>().OfType<IPingService>().Single());

            services.AddCoreComponents();
            services.AddBooksModule(_configuration);

            services.AddFeatureManagement();

            var healthChecksBuilder = services.AddHealthChecks()
                .AddBooksModule(_configuration);
            if (_configuration.GetValue<bool>("FeatureManagement:DockerCompose"))
            {
                healthChecksBuilder
                    .AddMySql(_configuration.GetConnectionString("MySqlDb"), tags: ["ready"])
                    .AddSqlServer(_configuration.GetConnectionString("MsSqlDb"), tags: ["ready"]);
            }
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddlewareForFeature<ConnectionInfoMiddleware>(FeatureFlags.ConnectionInfo.ToString());

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
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Simple Api V1");
                c.DocExpansion(DocExpansion.None);
            });

            app.InitBooksModule();
        }
    }
}
