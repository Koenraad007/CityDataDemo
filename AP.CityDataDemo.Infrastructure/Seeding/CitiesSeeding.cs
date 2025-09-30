using AP.CityDataDemo.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AP.CityDataDemo.Infrastructure.Seeding
{
    public static class CitiesSeeding
    {
        public static void Seed(this EntityTypeBuilder<City> modelBuilder)
        {
            modelBuilder.HasData(
                new City() { Id = 1, Name = "London", Population = 9800000, CountryId = 1 }
            );
        }
    }
}