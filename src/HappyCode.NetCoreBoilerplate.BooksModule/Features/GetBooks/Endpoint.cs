using HappyCode.NetCoreBoilerplate.BooksModule.Dtos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
                async Task<Ok<IEnumerable<BookDto>>> (
                    IDbConnection db,
                    CancellationToken ct
                ) =>
                {
                    return TypedResults.Ok(await db.GetBooksAsync(ct));
                });
        return endpoints;
    }
}
