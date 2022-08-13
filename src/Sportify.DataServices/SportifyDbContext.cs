using Sportify.DomainEntities;
using Sportify.DomainEntities.EventEntities;
using Sportify.DomainEntities.SportsGroundEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Sportify.DataServices;

public class SportifyDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
  public SportifyDbContext(DbContextOptions<SportifyDbContext> options) : base(options)
  {

  }

  public DbSet<Event> Events { get; set; }

  public DbSet<Venue> Venues { get; set; }

  public DbSet<EventCategory> EventCategories { get; set; }

  public DbSet<Country> Countries { get; set; }

  public DbSet<City> Cities { get; set; }

  public DbSet<SportsGroundLocation> SportsGroundLocations { get; set; }

  public DbSet<SportsGroundImage> SportsGroundImages { get; set; }

  public DbSet<SportsGroundType> SportsGroundTypes { get; set; }

  public DbSet<EventUser> EventUsers { get; set; }
}