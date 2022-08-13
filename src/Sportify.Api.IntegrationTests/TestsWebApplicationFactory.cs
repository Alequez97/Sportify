using Sportify.DataServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Sportify.Api.IntegrationTests.Extensions;
using Sportify.Api.Interfaces;
using Sportify.Api.Services;

namespace Sportify.Api.IntegrationTests
{
    public class TestsWebApplicationFactory : WebApplicationFactory<Startup>
    {
        public SportifyDbContext TestDbContext { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddTransient<IStorageService, TestsStorageService>();
                TestDbContext = services.BuildServiceProvider().GetRequiredService<SportifyDbContext>();
            });
        }
    }
}
