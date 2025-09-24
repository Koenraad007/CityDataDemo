using AP.CityDataDemo.Domain.Entities;
using AP.CityDataDemo.Application.Interfaces;

namespace AP.CityDataDemo.Infrastructure.Data;

public class DataSeeder : IDataSeeder
{
    private readonly ICityRepository _cityRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DataSeeder(ICityRepository cityRepository, ICountryRepository countryRepository, IUnitOfWork unitOfWork)
    {
        _cityRepository = cityRepository;
        _countryRepository = countryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task SeedAsync()
    {
        var existingCountries = await _countryRepository.GetAllCountriesAsync();
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
            new() { Name = "Nederland" },
            new() { Name = "België" },
            new() { Name = "Duitsland" },
            new() { Name = "Frankrijk" }
        };

        foreach (var country in countries)
        {
            await _countryRepository.AddCountryAsync(country);
        }
    }

    private async Task SeedCitiesAsync()
    {
        var countries = await _countryRepository.GetAllCountriesAsync();
        var netherlands = countries.First(c => c.Name == "Nederland");
        var belgium = countries.First(c => c.Name == "België");
        var germany = countries.First(c => c.Name == "Duitsland");
        var france = countries.First(c => c.Name == "Frankrijk");

        var cities = new List<City>
        {
            new("Amsterdam", 872757, netherlands.Id),
            new("Rotterdam", 651446, netherlands.Id),
            new("Den Haag", 548320, netherlands.Id),
            new("Utrecht", 361966, netherlands.Id),
            new("Antwerpen", 530504, belgium.Id),
            new("Brussel", 1208542, belgium.Id),
            new("Gent", 263614, belgium.Id),
            new("Berlijn", 3669491, germany.Id),
            new("München", 1488202, germany.Id),
            new("Parijs", 2161000, france.Id),
            new("Lyon", 518635, france.Id)
        };

        foreach (var city in cities)
        {
            await _cityRepository.AddCityAsync(city);
        }
    }
}
