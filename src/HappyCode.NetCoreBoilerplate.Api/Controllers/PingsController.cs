using System.Net;
using HappyCode.NetCoreBoilerplate.Api.BackgroundServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HappyCode.NetCoreBoilerplate.Api.Controllers
{
    [AllowAnonymous]
    [Route("api/pings")]
    public class PingsController : ApiControllerBase
    {
        private readonly IPingService _pingService;

        public PingsController(IPingService pingService)
        {
            _pingService = pingService;
        }

        [HttpGet("website")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public Task<IActionResult> GetWebsitePingStatusCodeAsync(
            CancellationToken cancellationToken = default)
        {
            var result = _pingService.WebsiteStatusCode;
            return Task.FromResult<IActionResult>(Ok($"{(int)result} ({result})"));
        }

        [HttpGet("random")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public Task<IActionResult> GetRandomStatusCodeAsync(
            CancellationToken cancellationToken = default)
        {
            var random = new Random(Guid.NewGuid().GetHashCode());
            int pretender;
            do
            {
                pretender = random.Next(100, 600);
            } while (!Enum.IsDefined(typeof(HttpStatusCode), pretender));
            return Task.FromResult<IActionResult>(Ok($"{pretender} ({(HttpStatusCode)pretender})"));
        }
    }
}
