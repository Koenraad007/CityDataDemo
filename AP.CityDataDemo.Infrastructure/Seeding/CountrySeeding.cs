using AP.CityDataDemo.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AP.CityDataDemo.Infrastructure.Seeding
{
    public static class CountrySeeding
    {
        public static void Seed(this EntityTypeBuilder<Country> modelBuilder)
        {
            modelBuilder.HasData(
                new Country() { Id = 1, Name = "Belgium" },
                new Country() { Id = 2, Name = "UK" },
                new Country() { Id = 3, Name = "France" },
                new Country() { Id = 4, Name = "Netherlands" },
                new Country() { Id = 5, Name = "Germany" }
            );
        }
    }
}