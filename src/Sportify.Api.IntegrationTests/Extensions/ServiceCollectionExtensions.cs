using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SportifyWebApi.IntegrationTests.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ReplaceDatabaseWithInMemory<T>(this IServiceCollection services) where T : DbContext
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<T>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
            services.AddDbContext<T>(options => options.UseInMemoryDatabase("TestDb"));
        }
    }
}
