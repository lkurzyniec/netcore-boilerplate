using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HappyCode.NetCoreBoilerplate.Api.Infrastructure.Middlewares;

[ExcludeFromCodeCoverage]
public class ConnectionInfoMiddleware(RequestDelegate next, ILogger<ConnectionInfoMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ConnectionInfoMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext httpContext)
    {
        _logger.LogDebug("===> Connection: {Connection}", new { ConnectionId = httpContext.Connection.Id, LocalIP = httpContext.Connection.LocalIpAddress, RemoteIP = httpContext.Connection.RemoteIpAddress });

        await _next.Invoke(httpContext);
    }
}
