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
    public class EditEventTests : SportifyWebApiIntegrationTestBase
    {
        EditEventRequest request = Builder<EditEventRequest>.CreateNew()
            .With(x => "Title is changed")
            .With(x => x.Id = 1)
            .With(x => x.CategoryId = 1)
            .With(x => x.CountryId = 1)
            .With(x => x.CityId = 1)
            .Build();

        [Fact]
        public async Task UnauthorizedAccess()
        {
            var response = await _testHttpClient.PutAsJsonAsync(Constants.Endpoints.Events.EditEvent, new object());
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task MissingRequiredField()
        {
            await AuthenticateAsync();

            request.Title = "";

            var response = await _testHttpClient.PutAsJsonAsync(Constants.Endpoints.Events.EditEvent, request);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task EditedSuccessfully()
        {
            await AuthenticateAsync();

            await PostEventAsync();

            var response = await _testHttpClient.PutAsJsonAsync(Constants.Endpoints.Events.EditEvent, request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var @event = _testDbContext.Events.FirstOrDefault();
            @event.Title.Should().Be(request.Title);
        }

        [Fact]
        public async Task EditWithUnpresentId()
        {
            await AuthenticateAsync();

            request.Id = 99; //does not exist

            var response = await _testHttpClient.PutAsJsonAsync(Constants.Endpoints.Events.EditEvent, request);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
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

        public class EditEventRequest
        {
            public int Id { get; set; }

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
