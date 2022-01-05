using System;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using FluentAssertions;
using Xunit;

namespace SportifyWebApi.IntegrationTests.Tests.Events
{
    public class DeleteEventTests : SportifyWebApiIntegrationTestBase
    {
        [Fact]
        public async Task UnauthorizedAccess()
        {
            var response = await _testHttpClient.DeleteAsync(Constants.Endpoints.Events.DeleteEvent.Replace("{id}", "1"));
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Delete()
        {
            await AuthenticateAsync();

            await PostEventAsync();

            _testDbContext.Events.Any().Should().BeTrue();

            var response = await _testHttpClient.DeleteAsync(Constants.Endpoints.Events.DeleteEvent.Replace("{id}", "1"));
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            _testDbContext.Events.Any().Should().BeFalse();
        }

        [Fact]
        public async Task DeleteWithUnpresentEventId()
        {
            await AuthenticateAsync();

            var response = await _testHttpClient.DeleteAsync(Constants.Endpoints.Events.DeleteEvent.Replace("{id}", "99"));
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteWithoutEventId()
        {
            await AuthenticateAsync();

            var response = await _testHttpClient.DeleteAsync(Constants.Endpoints.Events.DeleteEvent.Replace("{id}", ""));
            response.StatusCode.Should().Be(HttpStatusCode.MethodNotAllowed);
        }

        private async Task PostEventAsync()
        {
            CreateEventRequest request = Builder<CreateEventRequest>.CreateNew()
                .With(x => x.CategoryId = 1)
                .With(x => x.CountryId = 1)
                .With(x => x.CityId = 1)
                .Build();

            var response = await _testHttpClient.PostAsJsonAsync(Constants.Endpoints.Events.CreateEvent, request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        private class CreateEventRequest
        {
            public string Title { get; set; }

            public int CategoryId { get; set; }

            public string BriefDesc { get; set; }

            public string Description { get; set; }

            public int CountryId { get; set; }

            public int CityId { get; set; }

            public string Address { get; set; }

            public double Lat { get; set; }

            public double Lng { get; set; }

            public DateTime Date { get; set; }

            public bool IsGoing { get; set; }
        }
    }
}
