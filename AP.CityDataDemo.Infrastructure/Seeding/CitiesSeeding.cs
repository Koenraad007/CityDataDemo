using AP.CityDataDemo.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AP.CityDataDemo.Infrastructure.Seeding
{
    public static class CitiesSeeding
    {
        public static void Seed(this EntityTypeBuilder<City> modelBuilder)
        {
            modelBuilder.HasData(
                new City() { Id = 1, Name = "Brussels", Population = 1860000, CountryId = 1 },
                new City() { Id = 2, Name = "London", Population = 8900000, CountryId = 2 },
                new City() { Id = 3, Name = "Paris", Population = 2140000, CountryId = 3 },
                new City() { Id = 4, Name = "Amsterdam", Population = 872000, CountryId = 4 },
                new City() { Id = 5, Name = "Berlin", Population = 3769000, CountryId = 5 }
            );
        }
    }
}