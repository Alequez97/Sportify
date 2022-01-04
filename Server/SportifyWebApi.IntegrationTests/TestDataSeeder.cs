using DataServices;
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
        }
    }
}
