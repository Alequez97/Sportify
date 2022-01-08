using System;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace SportifyWebApi.IntegrationTests.Tests.Events
{
    public class CreateEventTests : SportifyWebApiIntegrationTestBase
    {
        CreateEventRequest request = Builder<CreateEventRequest>.CreateNew()
            .With(x => x.CategoryId = 1)
            .With(x => x.CountryId = 1)
            .With(x => x.CityId = 1)
            .Build();

        [Fact]
        public async Task UnauthorizedAccess()
        {
            var response = await _testHttpClient.PostAsJsonAsync(Constants.Endpoints.Events.CreateEvent, new object());
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task MissingRequiredField()
        {
            await AuthenticateAsync();

            request.Title = "";
            var response = await _testHttpClient.PostAsJsonAsync(Constants.Endpoints.Events.CreateEvent, request);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreatedSuccessfully()
        {
            await AuthenticateAsync();

            var response = await _testHttpClient.PostAsJsonAsync(Constants.Endpoints.Events.CreateEvent, request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var @event = _testDbContext.Events.Include(v => v.Venue).FirstOrDefault();

            @event.Title.Should().Be(request.Title);
            @event.Description.Should().Be(request.Description);
            @event.Venue.Address.Should().Be(request.Address);
            @event.Venue.Latitude.Should().Be(request.Lat);
            @event.Venue.Longitude.Should().Be(request.Lng);
        }

        [Fact]
        public async Task CreatedSuccessfullyAndAddedToContributorList()
        {
            await AuthenticateAsync();

            request.IsGoing = true;
            var response = await _testHttpClient.PostAsJsonAsync(Constants.Endpoints.Events.CreateEvent, request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var @event = _testDbContext.Events.Include(v => v.EventUsers).FirstOrDefault(e => e.Id == 1);

            @event.EventUsers.Count.Should().BeGreaterThan(0);
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
