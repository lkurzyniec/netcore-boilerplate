using HappyCode.NetCoreBoilerplate.BooksModule.Dtos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using System.Data;

namespace HappyCode.NetCoreBoilerplate.BooksModule.Features.GetBook;

internal static class Endpoint
{
    public static IEndpointRouteBuilder MapGetBookEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapGet(
                "/{id:int}",
                async Task<Results<Ok<BookDto>, NotFound>> (
                    int id,
                    IDbConnection db,
                    CancellationToken ct
                ) =>
                {
                    var book = await db.GetBookAsync(id, ct);
                    return book.Id is not null
                        ? TypedResults.Ok(book)
                        : TypedResults.NotFound();
                });
        // .Produces<BookDto>()
        // .Produces(StatusCodes.Status404NotFound);
        return endpoints;
    }
}
