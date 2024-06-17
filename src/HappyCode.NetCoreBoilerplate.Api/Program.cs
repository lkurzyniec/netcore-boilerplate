using HappyCode.NetCoreBoilerplate.Api;
using HappyCode.NetCoreBoilerplate.Api.Infrastructure.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

Log.Logger = SerilogConfigurator.CreateLogger();

try
{
    BannerConfigurator.Print(!Console.IsOutputRedirected);

    Log.Logger.Information("Starting up...");
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

static IHostBuilder CreateWebHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .UseSerilog()
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
