using System;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using HappyCode.NetCoreBoilerplate.Api.Infrastructure.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace HappyCode.NetCoreBoilerplate.Api
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = SerilogConfigurator.CreateLogger();

            try
            {
                Log.Logger.Information("Starting up");
                using var webHost = CreateWebHostBuilder(args).Build();
                await webHost.RunAsync();
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

        public static IHostBuilder CreateWebHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
