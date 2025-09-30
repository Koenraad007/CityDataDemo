using AP.CityDataDemo.Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using AP.CityDataDemo.Infrastructure.Seeding;

namespace AP.CityDataDemo.Infrastructure.Contexts
{
    public class CityDataDemoContext : DbContext
    {
        public CityDataDemoContext(DbContextOptions<CityDataDemoContext> options) : base(options) { }

        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Entity<City>().Seed();
            modelBuilder.Entity<Country>().Seed();
        }
    }
}