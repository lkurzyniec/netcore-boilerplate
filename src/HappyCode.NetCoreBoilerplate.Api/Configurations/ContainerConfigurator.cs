using Autofac;
using HappyCode.NetCoreBoilerplate.Core.RegisterModules;

namespace HappyCode.NetCoreBoilerplate.Api.Configurations
{
    public static class ContainerConfigurator
    {
        public static void RegisterModules(ContainerBuilder builder)
        {
            builder.RegisterModule<RepositoriesRegisterModule>();
        }
    }
}
