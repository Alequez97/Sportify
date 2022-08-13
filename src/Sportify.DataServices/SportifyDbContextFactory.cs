using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Sportify.DataServices;

public class SportifyDbContextFactory : IDesignTimeDbContextFactory<SportifyDbContext>
{
  public SportifyDbContext CreateDbContext(string[] args)
  {
    var optionsBuilder = new DbContextOptionsBuilder<SportifyDbContext>();
    optionsBuilder.UseSqlServer("Server = (localdb)\\mssqllocaldb; Database = sportify_db; Trusted_Connection = True"); //TODO: Replace with configuration read

    return new SportifyDbContext(optionsBuilder.Options);
  }
}
