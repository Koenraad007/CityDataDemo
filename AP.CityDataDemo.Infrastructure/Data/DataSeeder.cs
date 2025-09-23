using AP.CityDataDemo.Domain.Entities;
using AP.CityDataDemo.Domain.Interfaces;

namespace AP.CityDataDemo.Infrastructure.Data;

public class DataSeeder : IDataSeeder
{
    private readonly IUnitOfWork _unitOfWork;

    public DataSeeder(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task SeedAsync()
    {
        var existingCountries = await _unitOfWork.Countries.GetAllCountriesAsync();
        if (existingCountries.Any())
            return;

        await SeedCountriesAsync();
        await SeedCitiesAsync();
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task SeedCountriesAsync()
    {
        var countries = new List<Country>
        {
            new() { Id = 1, Name = "Nederland" },
            new() { Id = 2, Name = "België" },
            new() { Id = 3, Name = "Duitsland" },
            new() { Id = 4, Name = "Frankrijk" }
        };

        await _unitOfWork.Countries.AddCountriesAsync(countries);
    }

    private async Task SeedCitiesAsync()
    {
        var netherlands = await _unitOfWork.Countries.GetCountryByIdAsync(1);
        var belgium = await _unitOfWork.Countries.GetCountryByIdAsync(2);
        var germany = await _unitOfWork.Countries.GetCountryByIdAsync(3);
        var france = await _unitOfWork.Countries.GetCountryByIdAsync(4);

        var cities = new List<City>
        {
            new() { Id = 1, Name = "Amsterdam", Population = 872757, CountryId = 1, Country = netherlands! },
            new() { Id = 2, Name = "Rotterdam", Population = 651446, CountryId = 1, Country = netherlands! },
            new() { Id = 3, Name = "Den Haag", Population = 548320, CountryId = 1, Country = netherlands! },
            new() { Id = 4, Name = "Utrecht", Population = 361966, CountryId = 1, Country = netherlands! },
            new() { Id = 5, Name = "Antwerpen", Population = 530504, CountryId = 2, Country = belgium! },
            new() { Id = 6, Name = "Brussel", Population = 1208542, CountryId = 2, Country = belgium! },
            new() { Id = 7, Name = "Gent", Population = 263614, CountryId = 2, Country = belgium! },
            new() { Id = 8, Name = "Berlijn", Population = 3669491, CountryId = 3, Country = germany! },
            new() { Id = 9, Name = "München", Population = 1488202, CountryId = 3, Country = germany! },
            new() { Id = 10, Name = "Parijs", Population = 2161000, CountryId = 4, Country = france! },
            new() { Id = 11, Name = "Lyon", Population = 518635, CountryId = 4, Country = france! }
        };

        await _unitOfWork.Cities.AddCitiesAsync(cities);
    }
}
