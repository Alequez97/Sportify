using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using SportifyWebApi.IntegrationTests.Extensions;
using Xunit;

namespace SportifyWebApi.IntegrationTests.Tests.Map
{
    public class GetSportsGroundTypesTests : SportifyWebApiIntegrationTestBase
    {
        [Fact]
        public async Task UnauthorizedAccessAllowed()
        {
            var response = await _testHttpClient.GetAsync(Constants.Endpoints.Map.GetSportsGroundTypes);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task ResponseNotEmpty()
        {
            var response = await _testHttpClient.GetAsync(Constants.Endpoints.Map.GetSportsGroundTypes);
            var responseModel = await response.DeserializeResponseAsync<List<SportsGroundTypeResponse>>();

            responseModel.Should().NotBeEmpty();
        }

        private class SportsGroundTypeResponse
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}
