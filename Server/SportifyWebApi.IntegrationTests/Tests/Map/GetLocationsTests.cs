using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace SportifyWebApi.IntegrationTests.Tests.Map
{
    public class GetLocationsTests : SportifyWebApiIntegrationTestBase
    {
        private readonly string _urlWithoutDelta = $"{Constants.Endpoints.Map.GetLocations}?lat=56&lng=24";
        private readonly string _urlWithDelta = $"{Constants.Endpoints.Map.GetLocations}?lat=56&lng=24&delta=0.1";

        [Fact]
        public async Task UnauthorizedAccess()
        {
            var response = await _testHttpClient.GetAsync(_urlWithDelta);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task RequestWithoutDelta()
        {
            var response = await _testHttpClient.GetAsync(_urlWithoutDelta);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        private class SportsGroundGetLocationsRequest
        {
            public double Lat { get; set; }

            public double Lng { get; set; }

            public double Delta { get; set; }
        }

        private class SportsGroundGetLocationsResponse
        {
            public int Id { get; set; }

            public double Lat { get; set; }

            public double Lng { get; set; }

            public int TypeId { get; set; }

            public string TypeName { get; set; }

            public string Description { get; set; }

            public List<string> Images { get; set; }
        }
    }
}
