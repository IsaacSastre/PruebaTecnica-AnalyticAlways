using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace PTAnalitic.Infrastructure
{
    public class PTDbContextFactory : IDesignTimeDbContextFactory<PTDbContext>
    {


        public PTDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = ConfigurationExtensions.GetConnectionString(configuration, "PTContext");

            var optionsBuilder = new DbContextOptionsBuilder<PTDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new PTDbContext(optionsBuilder.Options);
        }
    }
}