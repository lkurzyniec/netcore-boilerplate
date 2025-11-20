using HappyCode.NetCoreBoilerplate.BooksModule.Dtos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
                async Task<NoContent> (
                    BookDto book,
                    IDbConnection db,
                    CancellationToken ct
                ) =>
                {
                    await db.UpsertBookAsync(book, ct);
                    return TypedResults.NoContent();
                })
            .WithDescription("This is UPSERT operation");
        return endpoints;
    }
}
