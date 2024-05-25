using System.Data;
using HappyCode.NetCoreBoilerplate.BooksModule.Features.DeleteBook;
using HappyCode.NetCoreBoilerplate.BooksModule.Features.GetBook;
using HappyCode.NetCoreBoilerplate.BooksModule.Features.GetBooks;
using HappyCode.NetCoreBoilerplate.BooksModule.Features.UpsertBook;
using HappyCode.NetCoreBoilerplate.BooksModule.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HappyCode.NetCoreBoilerplate.BooksModule;

public static class BooksModuleConfigurations
{
    public static IServiceCollection AddBooksModule(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddEndpointsApiExplorer()
            .AddSingleton<IDbConnection>(sp => new SqliteConnection(configuration.GetConnectionString("SqliteDb")))
            .AddSingleton<DbInitializer>()
            ;

    public static IEndpointRouteBuilder MapBooksModule(this IEndpointRouteBuilder endpoints)
        => endpoints
            .MapGroup("/api/books")
            .AddEndpointFilter<AuthFilter>()
            .MapGetBooksEndpoint()
            .MapGetBookEndpoint()
            .MapUpsertBookEndpoint()
            .MapDeleteBookEndpoint()
            ;

    public static IHealthChecksBuilder AddBooksModule(this IHealthChecksBuilder builder, IConfiguration configuration)
        => builder.AddSqlite(configuration.GetConnectionString("SqliteDb"));

    public static IApplicationBuilder InitBooksModule(this IApplicationBuilder app)
    {
        var initializer = app.ApplicationServices.GetRequiredService<DbInitializer>();
        initializer.Init();

        return app;
    }
}
