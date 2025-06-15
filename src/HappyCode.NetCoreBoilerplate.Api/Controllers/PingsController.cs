using System.Net;
using HappyCode.NetCoreBoilerplate.Api.BackgroundServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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

        [HttpGet("nth-string")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult GetNthString(List<string> strings, int n)
        {
            if (n < 0 || n >= strings.Count)
            {
                return BadRequest("Index must be non-negative and less than the size of the collection.");
            }
            return Ok(strings[n]);
        }
    }
}