using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DataServices;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using SportifyWebApi.IntegrationTests.Extensions;
using SportifyWebApi.Interfaces;
using SportifyWebApi.Services;

namespace SportifyWebApi.IntegrationTests
{
    public class SportifyWebApiIntegrationTestBase
    {
        protected readonly HttpClient _testHttpClient;
        protected SportifyDbContext _testDbContext;

        private readonly UserRequestTestModel _userModel = new UserRequestTestModel()
        {
            Username = "test-user",
            Email = "test@sportify.app",
            Password = "P@ssword1"
        };

        protected SportifyWebApiIntegrationTestBase()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.AddTransient<IStorageService, TestsStorageService>();
                        services.ReplaceDatabaseWithInMemory<SportifyDbContext>();
                        _testDbContext = services.BuildServiceProvider().GetRequiredService<SportifyDbContext>();
                        new TestDataSeeder(_testDbContext).SeedData();
                    });
                });

            _testHttpClient = appFactory.CreateClient();
        }

        protected async Task AuthenticateAsync()
        {
            var token = await GetJwtTokenAsync();
            _testHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        }

        private async Task<string> GetJwtTokenAsync()
        {
            await _testHttpClient.PostAsJsonAsync(Constants.Endpoints.Accounts.Register, _userModel);

            var user = _testDbContext.Users.FirstOrDefault(u => u.UserName == _userModel.Username);
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
