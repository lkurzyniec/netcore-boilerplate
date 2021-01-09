using HappyCode.NetCoreBoilerplate.Core.Repositories;
using HappyCode.NetCoreBoilerplate.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HappyCode.NetCoreBoilerplate.Core.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddCoreComponents(this IServiceCollection services)
        {
            services.AddTransient<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<ICarService, CarService>();

            return services;
        }
    }
}
