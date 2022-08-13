using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using FluentAssertions;
using Xunit;

namespace Sportify.Api.IntegrationTests.Tests.Events;

public class GetEventsTest : SportifyWebApiIntegrationTestBase
{
  [Fact]
  public async Task UnauthorizedAccessAllowed()
  {
    var response = await _testHttpClient.GetAsync(Constants.Endpoints.Events.GetEvents);
    response.StatusCode.Should().Be(HttpStatusCode.OK);
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
