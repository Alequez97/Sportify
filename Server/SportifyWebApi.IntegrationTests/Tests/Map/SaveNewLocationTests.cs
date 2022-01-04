using System.Collections.Generic;
using System.Net;
using System.Net.Http;
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
            var response = await PostValidLocationAsync();
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task ValidLocationUpload()
        {
            await AuthenticateAsync();

            var response = await PostValidLocationAsync();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseModel = await ExtractResponseModelAsync<SportsGroundSaveNewLocationResponse>(response);
            responseModel.Id.Should().BeGreaterThan(0);
            responseModel.Images.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task LocationWithoutTypeIdUpload()
        {
            await AuthenticateAsync();

            var response = await PostLocationWithoutTypeIdAsync();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        private Task<HttpResponseMessage> PostValidLocationAsync()
        {
            IList<KeyValuePair<string, string>> formData = new List<KeyValuePair<string, string>> {
                { new KeyValuePair<string, string>("TypeId", "1") },
                { new KeyValuePair<string, string>("Lat", "54") },
                { new KeyValuePair<string, string>("Lng", "27") }
            };

            return _testHttpClient.PostAsync(Constants.Endpoints.Map.SaveNewLocation, new FormUrlEncodedContent(formData));
        }

        private Task<HttpResponseMessage> PostLocationWithoutTypeIdAsync()
        {
            IList<KeyValuePair<string, string>> formData = new List<KeyValuePair<string, string>> {
                { new KeyValuePair<string, string>("Lat", "54") },
                { new KeyValuePair<string, string>("Lng", "27") }
            };

            return _testHttpClient.PostAsync(Constants.Endpoints.Map.SaveNewLocation, new FormUrlEncodedContent(formData));
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
