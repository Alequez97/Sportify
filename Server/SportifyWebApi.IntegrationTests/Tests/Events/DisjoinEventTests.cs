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
    public class DisjoinEventTests : SportifyWebApiIntegrationTestBase
    {
        [Fact]
        public async Task UnauthorizedAccess()
        {
            var response = await _testHttpClient.PostAsJsonAsync(Constants.Endpoints.Events.DisjoinEvent, new DisjoinEventRequest() { EventId = 1 });
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Disjoin()
        {
            await AuthenticateAsync();

            await PostEventAsync();

            var response = await _testHttpClient.PostAsJsonAsync(Constants.Endpoints.Events.DisjoinEvent, new DisjoinEventRequest() { EventId = 1 });
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var record = _appFactory.TestDbContext.EventUsers.FirstOrDefault();
            record.Should().NotBeNull();
            record.IsGoing.Should().BeFalse();
        }

        [Fact]
        public async Task DijoinWithoutEventId()
        {
            await AuthenticateAsync();

            var response = await _testHttpClient.PostAsJsonAsync(Constants.Endpoints.Events.DisjoinEvent, new object());
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task DisjoinWithUnpresentEventId()
        {
            await AuthenticateAsync();

            var response = await _testHttpClient.PostAsJsonAsync(Constants.Endpoints.Events.DisjoinEvent, new DisjoinEventRequest() { EventId = 99});
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        private async Task PostEventAsync()
        {
            CreateEventRequest request = Builder<CreateEventRequest>.CreateNew()
                .With(x => x.CategoryId = 1)
                .With(x => x.CountryId = 1)
                .With(x => x.CityId = 1)
                .With(x => x.IsGoing = true)
                .Build();

            var response = await _testHttpClient.PostAsJsonAsync(Constants.Endpoints.Events.CreateEvent, request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        private class DisjoinEventRequest
        {
            public int? EventId { get; set; }
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
