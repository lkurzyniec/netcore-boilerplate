using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HappyCode.NetCoreBoilerplate.BooksModule.IntegrationTests.Extensions
{
    internal static class HttpContentExtensions
    {
        public static HttpContent ToStringContent(this object source)
        {
            string json = source.ToJson();
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private static string ToJson(this object source)
        {
            return JsonConvert.SerializeObject(source, new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            });
        }
    }
}
