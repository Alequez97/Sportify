using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Sportify.Api.IntegrationTests.Extensions;
using Xunit;

namespace Sportify.Api.IntegrationTests.Tests.Events;

public class GetEventsCategoriesTests : SportifyWebApiIntegrationTestBase
{
  [Fact]
  public async Task UnauthorizedAccessAllowed()
  {
    var response = await _testHttpClient.GetAsync(Constants.Endpoints.Events.GetEventsCategories);
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  [Fact]
  public async Task ResponseNotEmpty()
  {
    var response = await _testHttpClient.GetAsync(Constants.Endpoints.Events.GetEventsCategories);
    var responseModel = await response.DeserializeResponseAsync<List<GetCategoriesResponse>>();

    responseModel.Should().NotBeEmpty();

  }

  public class GetCategoriesResponse
  {
    public int Id { get; set; }

    public string Name { get; set; }
  }
}
