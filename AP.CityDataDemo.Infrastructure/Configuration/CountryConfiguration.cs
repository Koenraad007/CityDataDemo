using AP.CityDataDemo.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AP.CityDataDemo.Infrastructure.Configuration
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.ToTable("tblCountries", "Country")
                   .HasKey(c => c.Id);

            builder.Property(c => c.Id)
                   .IsRequired()
                   .ValueGeneratedOnAdd();

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(100)
                   .IsUnicode(true);
        }
    }
}