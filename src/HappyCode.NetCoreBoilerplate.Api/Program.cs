using System;
using Autofac.Extensions.DependencyInjection;
using HappyCode.NetCoreBoilerplate.Api.Infrastructure.Configurations;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace HappyCode.NetCoreBoilerplate.Api
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = SerilogConfigurator.CreateLogger();

            try
            {
                Log.Logger.Information("Starting up");
                using var webHost = CreateWebHostBuilder(args).Build();
                webHost.Run();
            }
            catch (Exception ex)
            {
                Log.Logger.Fatal(ex, "Application start-up failed");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureServices(services => services.AddAutofac())
                .UseStartup<Startup>();
    }
}
