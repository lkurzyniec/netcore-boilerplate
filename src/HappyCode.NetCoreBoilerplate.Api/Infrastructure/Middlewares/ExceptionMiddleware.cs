using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HappyCode.NetCoreBoilerplate.Api.Infrastructure.Middlewares;

[ExcludeFromCodeCoverage]
public class ExceptionMiddleware(IWebHostEnvironment env, ILogger<ExceptionMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            if (env.IsDevelopment())
            {
                throw;
            }

            logger.LogCritical(exception, "An internal error has occurred");

            var errorResponse = new HttpValidationProblemDetails { Title = "An internal error has occurred" };
            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}
