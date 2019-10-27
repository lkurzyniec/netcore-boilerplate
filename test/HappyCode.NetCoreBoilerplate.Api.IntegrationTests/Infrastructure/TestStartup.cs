using Autofac;
using HappyCode.NetCoreBoilerplate.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HappyCode.NetCoreBoilerplate.Api.IntegrationTests.Infrastructure
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration)
            : base(configuration)
        {

        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                .AddJsonFormatters()
                .AddDataAnnotations()
                .SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddDbContext<EmployeesContext>(options =>
            {
                options.UseInMemoryDatabase("employees");
            });
            services.AddDbContext<CarsContext>(options =>
            {
                options.UseInMemoryDatabase("cars");
            });
        }

        public override void ConfigureContainer(ContainerBuilder builder)
        {
            base.ConfigureContainer(builder);

            // builder.RegisterType<SomeService>().As<ISomeService>();  //if needed override registration with own test fakes
        }

        public override void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var employeesContext = app.ApplicationServices.GetService<EmployeesContext>();
            EmployeesContextDataFeeder.Feed(employeesContext);

            var carsContext = app.ApplicationServices.GetService<CarsContext>();
            CarsContextDataFeeder.Feed(carsContext);

            app.UseMvc();
        }
    }
}
