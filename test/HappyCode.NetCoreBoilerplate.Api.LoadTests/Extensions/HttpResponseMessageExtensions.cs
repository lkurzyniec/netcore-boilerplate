using System.Net.Http;
using NBomber.Contracts;

namespace HappyCode.NetCoreBoilerplate.Api.LoadTests.Extensions
{
    internal static class HttpResponseMessageExtensions
    {
        public static Response GetCheckResult(this HttpResponseMessage response)
            => response.IsSuccessStatusCode ? Response.Ok() : Response.Fail(response.ReasonPhrase);
    }
}
