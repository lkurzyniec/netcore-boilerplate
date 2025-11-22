using System.Net;
using HappyCode.NetCoreBoilerplate.Api.BackgroundServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace HappyCode.NetCoreBoilerplate.Api.Controllers
{
    [AllowAnonymous]
    [Route("api/pings")]
    public class PingsController(IPingService pingService) : ApiControllerBase
    {
        [HttpGet("website")]
        public Task<Ok<string>> GetWebsitePingStatusCodeAsync(
            CancellationToken cancellationToken = default)
        {
            var result = pingService.WebsiteStatusCode;
            return Task.FromResult(TypedResults.Ok($"{(int)result} ({result})"));
        }

        [HttpGet("random")]
        public Task<Ok<string>> GetRandomStatusCodeAsync(
            CancellationToken cancellationToken = default)
        {
            var random = new Random(Guid.NewGuid().GetHashCode());
            int pretender;
            do
            {
                pretender = random.Next(100, 600);
            } while (!Enum.IsDefined(typeof(HttpStatusCode), pretender));
            return Task.FromResult(TypedResults.Ok($"{pretender} ({(HttpStatusCode)pretender})"));
        }
    }
}
