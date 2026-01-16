using System.Diagnostics.CodeAnalysis;

namespace HappyCode.NetCoreBoilerplate.Api.Infrastructure.Middlewares;

[ExcludeFromCodeCoverage]
public class BadHttpRequestExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (BadHttpRequestException exception)
        {
            var message = exception.Message.Replace("Did you mean to use a Service instead?", string.Empty);
            var errorResponse = new HttpValidationProblemDetails(new Dictionary<string, string[]> { ["Body"] = [message] });
            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}
