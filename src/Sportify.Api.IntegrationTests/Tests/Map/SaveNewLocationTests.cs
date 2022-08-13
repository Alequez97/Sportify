using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using SportifyWebApi.IntegrationTests.Extensions;
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

            using var formData = new MultipartFormDataContent();
            formData.Add(new StringContent("1"), "TypeId");
            formData.Add(new StringContent("54"), "Lat");
            formData.Add(new StringContent("27"), "Lng");

            var response = await _testHttpClient.PostAsync(Constants.Endpoints.Map.SaveNewLocation, formData);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseModel = await response.DeserializeResponseAsync<SportsGroundSaveNewLocationResponse>();
            responseModel.Id.Should().BeGreaterThan(0);
            responseModel.Images.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task ValidLocationWithImagesUpload()
        {
            await AuthenticateAsync();

            using var formData = new MultipartFormDataContent();

            using var image1 = File.OpenRead(@"Uploads/test-upload-1.jpg");
            using var image2 = File.OpenRead(@"Uploads/test-upload-2.jpg");

            using var imageContent1 = new StreamContent(image1);
            using var imageContent2 = new StreamContent(image2);

            formData.Add(imageContent1, "images", "test-upload-1.jpg");
            formData.Add(imageContent2, "images", "test-upload-2.jpg");
            formData.Add(new StringContent("1"), "TypeId");
            formData.Add(new StringContent("54"), "Lat");
            formData.Add(new StringContent("27"), "Lng");

            var response = await _testHttpClient.PostAsync(Constants.Endpoints.Map.SaveNewLocation, formData);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseModel = await response.DeserializeResponseAsync<SportsGroundSaveNewLocationResponse>();
            responseModel.Id.Should().BeGreaterThan(0);
            responseModel.Images.Count.Should().Be(2);
        }

        [Fact]
        public async Task LocationWithoutTypeIdUpload()
        {
            await AuthenticateAsync();

            using var formData = new MultipartFormDataContent();
            formData.Add(new StringContent("54"), "Lat");
            formData.Add(new StringContent("27"), "Lng");

            var response = await _testHttpClient.PostAsync(Constants.Endpoints.Map.SaveNewLocation, formData);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        private Task<HttpResponseMessage> PostValidLocationAsync()
        {
            IList<KeyValuePair<string, string>> formDataList = new List<KeyValuePair<string, string>> {
                { new KeyValuePair<string, string>("TypeId", "1") },
                { new KeyValuePair<string, string>("Lat", "54") },
                { new KeyValuePair<string, string>("Lng", "27") }
            };

            return _testHttpClient.PostAsync(Constants.Endpoints.Map.SaveNewLocation, new FormUrlEncodedContent(formDataList));
        }

        private class SportsGroundSaveNewLocationResponse
        {
            public int Id { get; set; }

            public List<string> Images { get; set; }
        }
    }
}
