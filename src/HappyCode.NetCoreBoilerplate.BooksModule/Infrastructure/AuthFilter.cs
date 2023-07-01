using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace HappyCode.NetCoreBoilerplate.BooksModule.Infrastructure
{
    internal class AuthFilter : IEndpointFilter
    {
        private readonly IConfiguration _configuration;

        public AuthFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            context.HttpContext.Request.Headers.TryGetValue("Authorization", out var values);
            var authorization = values.FirstOrDefault();

            if (authorization is null)
            {
                return Results.Unauthorized();
            }

            var key = Regex.Replace(authorization, @"APIKEY\s+", string.Empty, RegexOptions.IgnoreCase);

            var secretKey = _configuration.GetValue<string>("ApiKey:SecretKey");

            if (secretKey == key)
            {
                return await next(context);
            }
            return Results.Unauthorized();
        }
    }
}
