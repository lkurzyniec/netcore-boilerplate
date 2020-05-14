using Autofac;
using HappyCode.NetCoreBoilerplate.Core.RegisterModules;

namespace HappyCode.NetCoreBoilerplate.Api.Infrastructure.Registrations
{
    public static class ContainerRegistration
    {
        public static void RegisterModules(ContainerBuilder builder)
        {
            builder.RegisterModule<GeneralRegisterModule>();
        }
    }
}
