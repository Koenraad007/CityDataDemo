using AP.CityDataDemo.Domain.Entities;
using AP.CityDataDemo.Domain.Interfaces;

namespace AP.CityDataDemo.Infrastructure.Data;

public class DataSeeder : IDataSeeder
{
    private readonly IInMemoryDataStore _dataStore;

    public DataSeeder(IInMemoryDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public Task SeedAsync()
    {
        if (_dataStore.Countries.Any())
            return Task.CompletedTask;

        SeedCountries();
        SeedCities();

        return Task.CompletedTask;
    }

    private void SeedCountries()
    {
        var countries = new List<Country>
        {
            new() { Id = 1, Name = "Nederland" },
            new() { Id = 2, Name = "België" },
            new() { Id = 3, Name = "Duitsland" },
            new() { Id = 4, Name = "Frankrijk" }
        };

        _dataStore.Countries.AddRange(countries);
    }

    private void SeedCities()
    {
        var netherlands = _dataStore.Countries.First(c => c.Id == 1);
        var belgium = _dataStore.Countries.First(c => c.Id == 2);
        var germany = _dataStore.Countries.First(c => c.Id == 3);
        var france = _dataStore.Countries.First(c => c.Id == 4);

        var cities = new List<City>
        {
            new() { Id = 1, Name = "Amsterdam", Population = 872757, CountryId = 1, Country = netherlands },
            new() { Id = 2, Name = "Rotterdam", Population = 651446, CountryId = 1, Country = netherlands },
            new() { Id = 3, Name = "Den Haag", Population = 548320, CountryId = 1, Country = netherlands },
            new() { Id = 4, Name = "Utrecht", Population = 361966, CountryId = 1, Country = netherlands },
            new() { Id = 5, Name = "Antwerpen", Population = 530504, CountryId = 2, Country = belgium },
            new() { Id = 6, Name = "Brussel", Population = 1208542, CountryId = 2, Country = belgium },
            new() { Id = 7, Name = "Gent", Population = 263614, CountryId = 2, Country = belgium },
            new() { Id = 8, Name = "Berlijn", Population = 3669491, CountryId = 3, Country = germany },
            new() { Id = 9, Name = "München", Population = 1488202, CountryId = 3, Country = germany },
            new() { Id = 10, Name = "Parijs", Population = 2161000, CountryId = 4, Country = france },
            new() { Id = 11, Name = "Lyon", Population = 518635, CountryId = 4, Country = france }
        };

        _dataStore.Cities.AddRange(cities);
    }
}
