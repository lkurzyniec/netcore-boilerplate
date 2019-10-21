using System;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;

namespace HappyCode.NetCoreBoilerplate.Api
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = CreateLogger();

            try
            {
                Log.Logger.Information("Starting up");
                using var webHost = CreateWebHostBuilder(args).Build();
                webHost.Run();
            }
            catch (Exception ex)
            {
                Log.Logger.Fatal(ex, "Application start-up failed");
                Log.CloseAndFlush();
                throw;
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureServices(services => services.AddAutofac())
                .UseStartup<Startup>();

        private static Logger CreateLogger()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddJsonFile("appsettings.local.json", optional: true)
                .Build();

            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            return logger;
        }
    }
}
