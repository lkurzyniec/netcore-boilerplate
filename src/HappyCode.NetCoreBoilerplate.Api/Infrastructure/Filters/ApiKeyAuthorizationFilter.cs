using System.Linq;
using System.Text.RegularExpressions;
using HappyCode.NetCoreBoilerplate.Api.Infrastructure.Configurations;
using HappyCode.NetCoreBoilerplate.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;

namespace HappyCode.NetCoreBoilerplate.Api.Infrastructure.Filters
{
    public class ApiKeyAuthorizationFilter : IAsyncAuthorizationFilter
    {
        private static readonly Regex _apiKeyRegex = new Regex(@"^[Aa][Pp][Ii][Kk][Ee][Yy]\s+(?<ApiKey>.+)$", RegexOptions.Compiled);

        private readonly IOptions<ApiKeySettings> _options;
        private readonly IFeatureManager _featureManager;

        public ApiKeyAuthorizationFilter(IOptions<ApiKeySettings> options, IFeatureManager featureManager)
        {
            _options = options;
            _featureManager = featureManager;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!(await _featureManager.IsEnabledAsync(FeatureFlags.ApiKey)))
            {
                return;
            }
            bool hasAllowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<IAllowAnonymous>().Any();
            if (hasAllowAnonymous)
            {
                return;
            }

            if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var values))
            {
                context.Result = new UnauthorizedObjectResult("Authorization header is missing");
                return;
            }

            var authorization = values.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(authorization))
            {
                context.Result = new UnauthorizedObjectResult("Authorization header is empty");
                return;
            }

            var match = _apiKeyRegex.Match(authorization);
            if (!match.Success)
            {
                context.Result = new UnauthorizedObjectResult("ApiKey Authorization header value not match `ApiKey xxx-xxx`");
                return;
            }

            var apiKeyValue = match.Groups["ApiKey"].Value;
            // you can look into DB as well
            if (_options.Value.SecretKey != apiKeyValue)
            {
                context.Result = new UnauthorizedObjectResult("ApiKey Unauthorized");
                return;
            }
        }
    }
}
