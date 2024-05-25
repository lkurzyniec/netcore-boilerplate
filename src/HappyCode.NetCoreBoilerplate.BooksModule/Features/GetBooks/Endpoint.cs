using HappyCode.NetCoreBoilerplate.BooksModule.Dtos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Data;

namespace HappyCode.NetCoreBoilerplate.BooksModule.Features.GetBooks;

internal static class Endpoint
{
    public static IEndpointRouteBuilder MapGetBooksEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapGet(
                "/",
                async (
                    IDbConnection db,
                    CancellationToken ct
                ) =>
                {
                    return Results.Ok(await db.GetBooksAsync(ct));
                })
            .Produces<IEnumerable<BookDto>>()
            .WithTags("Books");
        return endpoints;
    }
}
