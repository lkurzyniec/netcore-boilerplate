using Autofac;
using HappyCode.NetCoreBoilerplate.Core.Repositories;

namespace HappyCode.NetCoreBoilerplate.Core.RegisterModules
{
    public class RepositoriesRegisterModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EmployeeRepository>().As<IEmployeeRepository>();
        }
    }
}
