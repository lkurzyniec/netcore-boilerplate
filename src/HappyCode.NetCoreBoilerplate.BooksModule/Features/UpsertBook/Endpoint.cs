using HappyCode.NetCoreBoilerplate.BooksModule.Dtos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Data;

namespace HappyCode.NetCoreBoilerplate.BooksModule.Features.UpsertBook;

internal static class Endpoint
{
    public static IEndpointRouteBuilder MapUpsertBookEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapPost(
                "/",
                async (
                    BookDto book,
                    IDbConnection db,
                    CancellationToken ct
                ) =>
                {
                    await db.UpsertBookAsync(book, ct);
                    return Results.NoContent();
                })
            .Produces(StatusCodes.Status204NoContent)
            .WithTags("Books");
        return endpoints;
    }
}
