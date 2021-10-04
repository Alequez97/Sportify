using DomainEntities;
using Microsoft.EntityFrameworkCore;

namespace DataServices
{
    public class SportifyDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\mssqllocaldb;Database=SportifyDb;Trusted_Connection=True");
        }
        public DbSet<Event> Events { get; set; }
        public DbSet<Venue> Venues { get; set; }
    }
}