using System.Data;
using Dapper;
using HappyCode.NetCoreBoilerplate.BooksModule.Dtos;

namespace HappyCode.NetCoreBoilerplate.BooksModule.Infrastructure;

internal class DbInitializer
{
    private static readonly string _createBooks = @$"
CREATE TABLE IF NOT EXISTS Books
(
    {nameof(BookDto.Id)}        INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    {nameof(BookDto.Title)}     TEXT
)
";

    private readonly IDbConnection _db;

    public DbInitializer(IDbConnection db)
    {
        _db = db;
    }

    public void Init()
    {
        _db.Execute(_createBooks);
    }
}
