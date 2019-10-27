using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HappyCode.NetCoreBoilerplate.Api.Infrastructure.Configurations;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
using HappyCode.NetCoreBoilerplate.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using HappyCode.NetCoreBoilerplate.Api.BackgroundServices;
using HappyCode.NetCoreBoilerplate.Api.Infrastructure.Filters;

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
            services.AddMvcCore(options =>
                {
                    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                    options.Filters.Add(typeof(ValidateModelStateFilter));
                })
                .AddApiExplorer()
                .AddDataAnnotations()
                .AddJsonFormatters()
                .SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddDbContext<EmployeesContext>(options => options.UseMySQL(_configuration.GetConnectionString("MySqlDb")), ServiceLifetime.Transient);
            services.AddDbContext<CarsContext>(options => options.UseSqlServer(_configuration.GetConnectionString("MsSqlDb")), ServiceLifetime.Transient);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Simple Api", Version = "v1" });
                c.DescribeAllParametersInCamelCase();
                c.OrderActionsBy(x => x.RelativePath);
            });

            services.Configure<PingWebsiteConfiguration>(_configuration.GetSection("PingWebsite"));
            services.AddHostedService<PingWebsiteBackgroundService>();
            services.AddHttpClient(nameof(PingWebsiteBackgroundService));

            services.AddHealthChecks();
        }

        public virtual void ConfigureContainer(ContainerBuilder builder)
        {
            ContainerConfigurator.RegisterModules(builder);
        }

        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHealthChecks("/healthcheck");

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Simple Api V1");
                c.DocExpansion(DocExpansion.None);
            });
        }
    }
}
