using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using Pomelo.EntityFrameworkCore.MySql;

namespace Loja.data
{
  public class LojaDbContextFactory : IDesignTimeDbContextFactory<LojaDbContext>
  {
    public LojaDbContext CreateDbContext(string[] args)
    {
      var optionsBuilder = new DbContextOptionsBuilder<LojaDbContext>();

      // Build configuration
      IConfigurationRoot configuration = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json")
          .Build();

      var connectionString = configuration.GetConnectionString("DefaultConnection");
      optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 26)));

      return new LojaDbContext(optionsBuilder.Options);
    }
  }
}
