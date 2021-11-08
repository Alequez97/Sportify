using DomainEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataServices
{
    public class SportifyDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\mssqllocaldb;Database=sportify_db;Trusted_Connection=True");
        }
        public DbSet<Event> Events { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<EventCategory> Categories { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Geolocation> Geolocations { get; set; }
        public DbSet<SportsGroundLocation> SportsGroundLocations { get; set; }
        public DbSet<SportsGroundType> SportsGroundCategories { get; set; }
    }
}