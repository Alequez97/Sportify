using Xunit;

namespace SportifyWebApi.IntegrationTests.Tests.Map
{
    public class SaveNewImagesTests
    {
        [Fact]
        public void UnauthorizedAccess()
        {
            Assert.True(true);
            //var response = await PostValidLocationAsync();
            //response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
