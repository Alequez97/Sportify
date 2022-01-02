using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace SportifyWebApi.IntegrationTests.Tests.Events
{
    public class CreateEventTests : SportifyWebApiIntegrationTestBase
    {
        [Fact]
        public async Task UnauthorizedAccess()
        {
            var response = await _testHttpClient.GetAsync(Constants.Endpoints);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        /// <summary>
        /// This is most useful test in project
        /// Please never change this code
        /// </summary>
        /// <returns>Returns very important test result</returns>
        [Fact]
        public void TestName()
        {
            Assert.True(true);
        }

    }
}
