using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using FluentAssertions;
using Xunit;

namespace SportifyWebApi.IntegrationTests.Tests.Events
{
    public class CreateEventTests : SportifyWebApiIntegrationTestBase
    {
        CreateEventRequest request = Builder<CreateEventRequest>.CreateNew()
            .With(x => x.CategoryId = 1).Build();

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

            var @event = await _testHttpClient.GetAsync(Constants.Endpoints.Events.GetEvent.Replace("{id}", "1"));
            var eventModel = await ExtractResponseModelAsync<GetEventResponse>(@event);

            eventModel.Title.Should().Be(request.Title);
            eventModel.Description.Should().Be(request.Description);
            eventModel.Venue.Address.Should().Be(request.Address);
            eventModel.Venue.Lat.Should().Be(request.Lat);
            eventModel.Venue.Lng.Should().Be(request.Lng);
            eventModel.Venue.Lng.Should().Be(request.Lng);
        }

        [Fact]
        public async Task CreatedSuccessfullyAndAddedToContributorList()
        {
            await AuthenticateAsync();

            request.IsGoing = true;
            var response = await _testHttpClient.PostAsJsonAsync(Constants.Endpoints.Events.CreateEvent, request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var @event = await _testHttpClient.GetAsync(Constants.Endpoints.Events.GetEvent.Replace("{id}", "1"));
            var eventModel = await ExtractResponseModelAsync<GetEventResponse>(@event);

            eventModel.Contributors.Count.Should().BeGreaterThan(0);
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

        private class GetEventResponse
        {
            public string Title { get; set; }

            public string CategoryName { get; set; }

            public string BriefDesc { get; set; }

            public string Description { get; set; }

            public GetEventVenueDto Venue { get; set; }

            public string Date { get; set; }

            public string CreatorUsername { get; set; }

            public int CreatorId { get; set; }

            public List<GetEventContributorDto> Contributors { get; set; }

            public class GetEventContributorDto
            {
                public int Id { get; set; }
                public string Username { get; set; }
            }

            public class GetEventVenueDto
            {
                public string Country { get; set; }

                public string City { get; set; }

                public string Address { get; set; }

                public double Lat { get; set; }

                public double Lng { get; set; }
            }
        }
    }
}
