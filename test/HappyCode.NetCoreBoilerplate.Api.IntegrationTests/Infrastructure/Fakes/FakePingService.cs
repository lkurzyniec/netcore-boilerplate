using System.Net;
using HappyCode.NetCoreBoilerplate.Api.BackgroundServices;

namespace HappyCode.NetCoreBoilerplate.Api.IntegrationTests.Infrastructure.Fakes
{
    public class FakePingService : IPingService
    {
        internal const HttpStatusCode Result = HttpStatusCode.EarlyHints;

        public HttpStatusCode WebsiteStatusCode => Result;
    }
}
