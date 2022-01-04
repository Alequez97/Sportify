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
            SeedCities();
            SeedCountries();
            SeedEventCategories();

            _dbContext.SaveChanges();
        }

        private void SeedSportsGroundTypes()
        {
            _dbContext.SportsGroundTypes.Add(new SportsGroundType()
            {
                Name = "Basketball field"
            });
        }

        private void SeedCountries()
        {
            _dbContext.Countries.Add(new Country()
            {
                Name = "Latvia"
            });

            _dbContext.Countries.Add(new Country()
            {
                Name = "Estonia"
            });
        }

        private void SeedCities()
        {
            _dbContext.Cities.Add(new City()
            {
                CountryId = 1,
                Name = "Riga"
            });

            _dbContext.Cities.Add(new City()
            {
                CountryId = 2,
                Name = "Talinn"
            });
        }

        private void SeedEventCategories()
        {
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
