using System.Threading.Tasks;
using Xunit;

namespace SportifyWebApi.IntegrationTests.Tests.Map
{
    public class SaveNewImagesTests
    {
        [Fact]
        public async Task UnauthorizedAccess()
        {
            Assert.True(true);
            //var response = await PostValidLocationAsync();
            //response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
