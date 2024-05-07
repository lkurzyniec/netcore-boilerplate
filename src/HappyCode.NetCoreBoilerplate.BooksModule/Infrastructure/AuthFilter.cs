using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace HappyCode.NetCoreBoilerplate.BooksModule.Infrastructure
{
    internal class AuthFilter : IEndpointFilter
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public AuthFilter(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            if (!_env.IsProduction())
            {
                return await next(context);
            }

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
