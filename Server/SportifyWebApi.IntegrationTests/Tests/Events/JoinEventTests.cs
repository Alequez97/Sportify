using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace SportifyWebApi.IntegrationTests.Tests.Events
{
    public class JoinEventTests : SportifyWebApiIntegrationTestBase
    {
        [Fact]
        public async Task UnauthorizedAccess()
        {
            var response = await _testHttpClient.PostAsJsonAsync(Constants.Endpoints.Events.JoinEvent, new JoinEventRequest(){ EventId = 1 });
        }

        private class JoinEventRequest
        {
            public int EventId { get; set; }
        }
    }
}
