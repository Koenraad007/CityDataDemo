using AP.CityDataDemo.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AP.CityDataDemo.Infrastructure.Configuration
{
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable("tblCities", "City")
                   .HasKey(c => c.Id);

            builder.Property(c => c.Id)
                   .IsRequired()
                   .ValueGeneratedOnAdd();

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(100)
                   .IsUnicode(true);

            builder.Property(c => c.Population)
                    .IsRequired()
                    .HasColumnType("int");

            builder.HasOne(c => c.Country)
                   .WithMany()
                   .HasForeignKey(c => c.CountryId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}