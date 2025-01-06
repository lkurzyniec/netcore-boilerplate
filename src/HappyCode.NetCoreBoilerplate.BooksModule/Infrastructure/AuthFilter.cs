using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;

namespace HappyCode.NetCoreBoilerplate.BooksModule.Infrastructure
{
    internal partial class AuthFilter : IEndpointFilter
    {
        [GeneratedRegex(@"APIKEY\s+", RegexOptions.IgnoreCase)]
        private static partial Regex ApiKeyRegex();

        private readonly IConfiguration _configuration;
        private readonly IFeatureManager _featureManager;

        public AuthFilter(IConfiguration configuration, IFeatureManager featureManager)
        {
            _configuration = configuration;
            _featureManager = featureManager;
        }

        public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            if (!await _featureManager.IsEnabledAsync("ApiKey"))
            {
                return await next(context);
            }

            context.HttpContext.Request.Headers.TryGetValue("Authorization", out var values);
            var authorization = values.FirstOrDefault();

            if (authorization is null)
            {
                return Results.Unauthorized();
            }

            var key = ApiKeyRegex().Replace(authorization, string.Empty);

            var secretKey = _configuration.GetValue<string>("ApiKey:SecretKey");

            if (secretKey == key)
            {
                return await next(context);
            }
            return Results.Unauthorized();
        }
    }
}
