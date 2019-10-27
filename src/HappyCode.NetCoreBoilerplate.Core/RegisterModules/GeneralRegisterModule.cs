using Autofac;
using HappyCode.NetCoreBoilerplate.Core.Repositories;
using HappyCode.NetCoreBoilerplate.Core.Services;

namespace HappyCode.NetCoreBoilerplate.Core.RegisterModules
{
    public class GeneralRegisterModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EmployeeRepository>().As<IEmployeeRepository>();
            builder.RegisterType<CarService>().As<ICarService>();
        }
    }
}
