using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Data;

namespace HappyCode.NetCoreBoilerplate.BooksModule.Features.DeleteBook;

internal static class Endpoint
{
    public static IEndpointRouteBuilder MapDeleteBookEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapDelete(
                "/{id:int}",
                async (
                    int id,
                    IDbConnection db,
                    CancellationToken ct
                ) =>
                {
                    var affected = await db.DeleteBookAsync(id, ct);
                    return affected > 0
                        ? Results.NoContent()
                        : Results.NotFound();
                })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags("Books");
        return endpoints;
    }
}
