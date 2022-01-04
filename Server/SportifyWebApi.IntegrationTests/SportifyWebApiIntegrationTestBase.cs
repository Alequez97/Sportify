using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DataServices;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SportifyWebApi.IntegrationTests.Extensions;

namespace SportifyWebApi.IntegrationTests
{
    public class SportifyWebApiIntegrationTestBase
    {
        protected readonly HttpClient _testHttpClient;
        protected SportifyDbContext _testDbContext;

        private readonly string _username = "test-user";
        private readonly string _email = "test@sportify.app";
        private readonly string _password = "P@ssword1";

        protected SportifyWebApiIntegrationTestBase()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        // This replaces real database with in-memory database. Real database also can be used, but in that case clean up methods will be required 
                        // if test will be run on real env
                        //
                        // Comment all lines after this comment to use real database
                        services.ReplaceDatabaseWithInMemory<SportifyDbContext>();
                        _testDbContext = services.BuildServiceProvider().GetRequiredService<SportifyDbContext>();
                        new TestDataSeeder(_testDbContext).SeedData();
                    });
                });

            _testHttpClient = appFactory.CreateClient();
        }

        protected async Task<T> ExtractResponseModelAsync<T>(HttpResponseMessage response)
        {
            var responseAsString = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<T>(responseAsString);

            return responseModel;
        }

        protected async Task AuthenticateAsync()
        {
            _testHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtTokenAsync());
        }

        private async Task<string> GetJwtTokenAsync()
        {
            var userModel = new UserRequestTestModel()
            { 
                Username = _username,
                Email = _email,
                Password = _password
            };
            await _testHttpClient.PostAsJsonAsync(Constants.Endpoints.Accounts.Register, userModel);
            var loginResponse = await _testHttpClient.PostAsJsonAsync(Constants.Endpoints.Accounts.Login, userModel);
            var loginResponseModel = await ExtractResponseModelAsync<UserResponseTestModel>(loginResponse);

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
