using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HappyCode.NetCoreBoilerplate.Api.Infrastructure.Filters
{
    [ExcludeFromCodeCoverage]
    public class HttpGlobalExceptionFilter(IWebHostEnvironment env, ILogger<HttpGlobalExceptionFilter> logger) : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (env.IsDevelopment())
            {
                throw context.Exception;
            }

            logger.LogError(context.Exception, "An API internal error has occurred");

            var errorResponse = new HttpValidationProblemDetails { Title = "An internal error has occurred" };
            context.Result = new ObjectResult(errorResponse) { StatusCode = StatusCodes.Status500InternalServerError };

            context.ExceptionHandled = true;
        }
    }
}
