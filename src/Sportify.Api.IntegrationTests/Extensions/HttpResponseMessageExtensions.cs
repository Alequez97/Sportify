using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SportifyWebApi.IntegrationTests.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<T> DeserializeResponseAsync<T>(this HttpResponseMessage response)
        {
            var responseAsString = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<T>(responseAsString);

            return responseModel;
        }
    }
}
