using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using SportifyWebApi.IntegrationTests.Extensions;

namespace SportifyWebApi.IntegrationTests
{
    public class SportifyWebApiIntegrationTestBase
    {
        protected readonly HttpClient _testHttpClient;
        protected readonly TestsWebApplicationFactory _appFactory;

        private readonly UserRequestTestModel _userModel = new UserRequestTestModel()
        {
            Username = "test-user",
            Email = "test@sportify.app",
            Password = "P@ssword1"
        };

        protected SportifyWebApiIntegrationTestBase()
        {
            _appFactory = new TestsWebApplicationFactory();
            _testHttpClient = _appFactory.CreateClient();
        }

        protected async Task AuthenticateAsync()
        {
            var token = await GetJwtTokenAsync();
            _testHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        }

        private async Task<string> GetJwtTokenAsync()
        {
            await _testHttpClient.PostAsJsonAsync(Constants.Endpoints.Accounts.Register, _userModel);

            var user = _appFactory.TestDbContext.Users.FirstOrDefault(u => u.UserName == _userModel.Username);
            user.Should().NotBeNull();

            var loginResponse = await _testHttpClient.PostAsJsonAsync(Constants.Endpoints.Accounts.Login, _userModel);
            var loginResponseModel = await loginResponse.DeserializeResponseAsync<UserResponseTestModel>();
            loginResponseModel.Token.Should().NotBeNullOrEmpty();

            return loginResponseModel.Token;
        }

        private class UserRequestTestModel
        {
            public string Username { get; set; }

            public string Email { get; set; }

            public string Password { get; set; }
        }

        private class UserResponseTestModel
        {
            public string Token { get; set; }
        }
    }
}
