using HappyCode.NetCoreBoilerplate.Api.BackgroundServices;
using HappyCode.NetCoreBoilerplate.Api.Infrastructure.Filters;
using HappyCode.NetCoreBoilerplate.Api.IntegrationTests.Infrastructure.DataFeeders;
using HappyCode.NetCoreBoilerplate.Api.IntegrationTests.Infrastructure.Fakes;
using HappyCode.NetCoreBoilerplate.Core;
using HappyCode.NetCoreBoilerplate.Core.Registrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement;

namespace HappyCode.NetCoreBoilerplate.Api.IntegrationTests.Infrastructure
{
    internal class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration)
            : base(configuration)
        {

        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services
                .AddHttpContextAccessor()
                .AddMvcCore(options =>
                {
                    options.Filters.Add<ValidateModelStateFilter>();
                })
                .AddDataAnnotations();

            services.AddCoreComponents();
            services.AddSingleton<IPingService, FakePingService>();  //override registration with own fakes

            services.AddFeatureManagement();

            services.AddDbContext<EmployeesContext>(options =>
            {
                options.UseInMemoryDatabase("employees");
            });
            services.AddDbContext<CarsContext>(options =>
            {
                options.UseInMemoryDatabase("cars");
            });
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var employeesContext = app.ApplicationServices.GetService<EmployeesContext>();
            EmployeesContextDataFeeder.Feed(employeesContext);

            var carsContext = app.ApplicationServices.GetService<CarsContext>();
            CarsContextDataFeeder.Feed(carsContext);

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
