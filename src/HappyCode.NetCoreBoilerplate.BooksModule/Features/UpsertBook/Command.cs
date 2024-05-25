using System.Data;
using Dapper;
using HappyCode.NetCoreBoilerplate.BooksModule.Dtos;

namespace HappyCode.NetCoreBoilerplate.BooksModule.Features.UpsertBook;

internal static class Command
{
    private static readonly string _upsertBook = @$"
INSERT INTO Books ({nameof(BookDto.Id)}, {nameof(BookDto.Title)})
    VALUES(@id, @title)
    ON CONFLICT ({nameof(BookDto.Id)}) DO UPDATE SET {nameof(BookDto.Title)} = excluded.{nameof(BookDto.Title)}
";

    private static readonly string _nextId = @$"
SELECT IFNULL(MAX({nameof(BookDto.Id)}), 0) + 1
    FROM Books
";

    public static async Task UpsertBookAsync(this IDbConnection db, BookDto book, CancellationToken cancellationToken)
    {
        var id = book.Id ?? await db.ExecuteScalarAsync<int>(new CommandDefinition(_nextId, cancellationToken: cancellationToken));

        await db.ExecuteAsync(new CommandDefinition(_upsertBook, new
        {
            id = id,
            title = book.Title,
        }, cancellationToken: cancellationToken));
    }
}
