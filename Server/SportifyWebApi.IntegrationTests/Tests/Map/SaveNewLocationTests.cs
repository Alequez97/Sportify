using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace SportifyWebApi.IntegrationTests.Tests.Map
{
    public class SaveNewLocationTests : SportifyWebApiIntegrationTestBase
    {
        [Fact]
        public async Task UnauthorizedAccess()
        {
            var requestModel = new SportsGroundSaveNewLocationRequest()
            {
                TypeId = 1,
                Lat = 50,
                Lng = 50
            };
            var response = await _testHttpClient.PostAsJsonAsync(Constants.Endpoints.Map.SaveNewLocation, requestModel);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        private class SportsGroundSaveNewLocationRequest
        {
            public int TypeId { get; set; }

            public string Description { get; set; }

            public string Country { get; set; }

            public string City { get; set; }

            public string District { get; set; }

            public string Street { get; set; }

            public string HouseNumber { get; set; }

            public double Lat { get; set; }

            public double Lng { get; set; }

            public List<IFormFile> Images { get; set; }
        }

        private class SportsGroundSaveNewLocationResponse
        {
            public int Id { get; set; }

            public List<string> Images { get; set; }
        }
    }
}
