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
                async Task<Results<CreatedAtRoute<BookDto>, BadRequest<HttpValidationProblemDetails>>> (
                    BookDto book,
                    IDbConnection db,
                    CancellationToken ct
                ) =>
                {
                    var id = await db.UpsertBookAsync(book, ct);
                    book.Id = id;
                    return TypedResults.CreatedAtRoute(book, nameof(GetBook), new { id });
                })
            .WithDescription("This is UPSERT operation");
        return endpoints;
    }
}
