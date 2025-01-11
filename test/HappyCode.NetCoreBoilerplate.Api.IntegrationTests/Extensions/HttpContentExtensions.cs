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
        };

        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
        {
            string json = await content.ReadAsStringAsync();
            var value = JsonSerializer.Deserialize<T>(json);
            return value;
        }

        public static HttpContent ToStringContent(this object source)
        {
            string json = source.ToJson();
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private static string ToJson(this object source)
            => JsonSerializer.Serialize(source, _jsonSerializerOptions);
    }
}
