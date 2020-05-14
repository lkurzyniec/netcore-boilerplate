using System.Linq;
using System.Text.RegularExpressions;
using HappyCode.NetCoreBoilerplate.Api.Infrastructure.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace HappyCode.NetCoreBoilerplate.Api.Infrastructure.Filters
{
    public class ApiKeyAuthorizationFilter : IAuthorizationFilter
    {
        private static readonly Regex _apiKeyRegex = new Regex(@"^[Aa][Pp][Ii][Kk][Ee][Yy]\s+(?<ApiKey>.+)$", RegexOptions.Compiled);

        private readonly IOptions<ApiKeySettings> _options;

        public ApiKeyAuthorizationFilter(IOptions<ApiKeySettings> options)
        {
            _options = options;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
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
