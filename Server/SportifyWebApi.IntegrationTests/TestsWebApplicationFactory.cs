using DataServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using SportifyWebApi.IntegrationTests.Extensions;
using SportifyWebApi.Interfaces;
using SportifyWebApi.Services;

namespace SportifyWebApi.IntegrationTests
{
    public class TestsWebApplicationFactory : WebApplicationFactory<Startup>
    {
        public SportifyDbContext TestDbContext { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddTransient<IStorageService, TestsStorageService>();
                services.ReplaceDatabaseWithDockerAsync<SportifyDbContext>().Wait();
                TestDbContext = services.BuildServiceProvider().GetRequiredService<SportifyDbContext>();
            });
        }
    }
}
