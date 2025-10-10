using AP.CityDataDemo.Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using AP.CityDataDemo.Infrastructure.Seeding;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AP.CityDataDemo.Infrastructure.Contexts
{
    public class CityDataDemoContext : DbContext
    {
        public CityDataDemoContext(DbContextOptions<CityDataDemoContext> options) : base(options)
        {
            // try
            // {
            //     var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
            //     if (databaseCreator != null)
            //     {
            //         if (!databaseCreator.CanConnect()) databaseCreator.Create();
            //         if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
            //     }
            // }
            // catch (Exception ex)
            // {
            //     Console.WriteLine(ex.Message);
            // }
        }

        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }

        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        //     modelBuilder.Entity<City>().Seed();
        //     modelBuilder.Entity<Country>().Seed();
        // }
    }
}