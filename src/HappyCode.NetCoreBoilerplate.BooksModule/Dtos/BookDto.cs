using System.ComponentModel.DataAnnotations;

namespace HappyCode.NetCoreBoilerplate.BooksModule.Dtos;

public record struct BookDto (int? Id, [property: Required] string Title);
