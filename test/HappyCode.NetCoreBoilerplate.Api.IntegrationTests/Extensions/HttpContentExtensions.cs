using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HappyCode.NetCoreBoilerplate.Api.IntegrationTests.Extensions
{
    internal static class HttpContentExtensions
    {
        private static JsonSerializerOptions _jsonSerializerOptions = new ()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public static HttpContent ToStringContent(this object source)
            => new StringContent(JsonSerializer.Serialize(source, _jsonSerializerOptions), Encoding.UTF8, "application/json");
    }
}
