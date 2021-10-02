using DomainEntities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace DataServices
{
    public class SportifyDbContext : DbContext
    {
        public SportifyDbContext([NotNull] DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Venue> Venues { get; set; }
    }
}
