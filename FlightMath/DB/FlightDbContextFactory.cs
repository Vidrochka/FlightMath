using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace FlightMath.DB
{
    public class FlightDbContextFactory : IDesignTimeDbContextFactory<FlightDbContext>
    {
        public FlightDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<FlightDbContext> optionsBuilder = new DbContextOptionsBuilder<FlightDbContext>();
            IConfigurationRoot config = BuildConfiguration("appsettings.json");

            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"),
                opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(90).TotalSeconds));
            return new FlightDbContext(optionsBuilder.Options);
        }

        private IConfigurationRoot BuildConfiguration(string configurationFileName)
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile(configurationFileName);
            return builder.Build();
        }
    }
}