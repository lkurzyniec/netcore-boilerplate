using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace HappyCode.NetCoreBoilerplate.Api.LoadTests.Extensions
{
    internal static class StringExtensions
    {
        public static StringContent ToStringContent(this string source) =>
            new StringContent(source, Encoding.UTF8, "application/json");

        public static T Deserialize<T>(this string source) =>
            JsonSerializer.Deserialize<T>(source, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}
