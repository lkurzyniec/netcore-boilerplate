using HappyCode.NetCoreBoilerplate.BooksModule.Dtos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
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
                async (
                    int id,
                    IDbConnection db,
                    CancellationToken ct
                ) =>
                {
                    var book = await db.GetBookAsync(id, ct);
                    return book.Id is not null
                        ? Results.Ok(book)
                        : Results.NotFound();
                })
            .Produces<BookDto>()
            .Produces(StatusCodes.Status404NotFound)
            .WithTags("Books");
        return endpoints;
    }
}
