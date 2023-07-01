using HappyCode.NetCoreBoilerplate.BooksModule.Dtos;
using HappyCode.NetCoreBoilerplate.BooksModule.Infrastructure;
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
                    IDbConnection db,
                    BookDto book,
                    CancellationToken ct
                ) =>
                {
                    await db.UpsertBookAsync(book, ct);
                    return TypedResults.NoContent();
                })
            .Produces(StatusCodes.Status204NoContent)
            .WithTags("Books");
        return endpoints;
    }
}
