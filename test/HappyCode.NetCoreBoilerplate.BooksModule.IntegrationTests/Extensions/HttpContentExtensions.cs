using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HappyCode.NetCoreBoilerplate.BooksModule.IntegrationTests.Extensions
{
    internal static class HttpContentExtensions
    {
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public static HttpContent ToStringContent(this object source)
        {
            string json = source.ToJson();
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private static string ToJson(this object source)
            => JsonSerializer.Serialize(source, _jsonSerializerOptions);
    }
}
