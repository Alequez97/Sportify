using DomainEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataServices
{
    public class SportifyDbContext : IdentityDbContext<User>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\mssqllocaldb;Database=sportify_db;Trusted_Connection=True");
        }
        public DbSet<Event> Events { get; set; }
        public DbSet<Venue> Venues { get; set; }
    }
}