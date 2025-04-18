using HappyCode.NetCoreBoilerplate.Api;
using HappyCode.NetCoreBoilerplate.Api.Infrastructure.Configurations;
using Microsoft.AspNetCore.Builder;
using Serilog;

Log.Logger = SerilogConfigurator.CreateLogger();

try
{
    BannerConfigurator.Print(!Console.IsOutputRedirected);

    Log.Logger.Information("Starting up...");
    var builder = WebApplication.CreateBuilder();
    var startup = new Startup(builder.Configuration);
    startup.ConfigureServices(builder.Services);
    using var app = builder.Build();
    startup.Configure(app, builder.Environment);
    await app.RunAsync();
    Log.Logger.Debug("I'm done, see ya later!");
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
