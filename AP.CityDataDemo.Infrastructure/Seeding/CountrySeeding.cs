using AP.CityDataDemo.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AP.CityDataDemo.Infrastructure.Seeding
{
    public static class CountrySeeding
    {
        public static void Seed(this EntityTypeBuilder<Country> modelBuilder)
        {
            modelBuilder.HasData(
                new Country() { Id = 1, Name = "UK" }
            );
        }
    }
}