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

        var count = _db.ExecuteScalar<int>("SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name = 'Books'");
        if (count == 0)
        {
            throw new ApplicationException("SQLite DB not initialized properly");
        }
    }
}
