using DataServices;
using DomainEntities;
using DomainEntities.EventEntities;
using DomainEntities.SportsGroundEntities;

namespace SportifyWebApi.IntegrationTests
{
    internal sealed class TestDataSeeder
    {
        private readonly SportifyDbContext _dbContext;

        public TestDataSeeder(SportifyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void SeedData()
        {
            SeedSportsGroundTypes();

            _dbContext.SaveChanges();
        }

        private void SeedSportsGroundTypes()
        {
            _dbContext.SportsGroundTypes.Add(new SportsGroundType()
            {
                Name = "Basketball field"
            });

            _dbContext.Countries.Add(new Country()
            {
                Name = "Latvia"
            });

            _dbContext.Countries.Add(new Country()
            {
                Name = "Estonia"
            });

            _dbContext.Cities.Add(new City()
            {
                CountryId = 1,
                Name = "Talin"
            });

            _dbContext.EventCategories.Add(new EventCategory()
            {
                Name = "Basketball"
            });

            _dbContext.EventCategories.Add(new EventCategory()
            {
                Name = "Voleyball"
            });
        }
    }
}
