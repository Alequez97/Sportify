using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace SportifyWebApi.IntegrationTests.Tests.Map
{
    public class SaveNewImagesTests : SportifyWebApiIntegrationTestBase
    {
        [Fact]
        public async Task UnauthorizedAccess()
        {
            var response = await _testHttpClient.PostAsync(Constants.Endpoints.Map.SaveNewImages, new MultipartFormDataContent());
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
