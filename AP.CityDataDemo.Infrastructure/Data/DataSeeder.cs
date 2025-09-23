using AP.CityDataDemo.Domain.Entities;
using AP.CityDataDemo.Domain.Interfaces;

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
            new() { Id = 1, Name = "Nederland" },
            new() { Id = 2, Name = "België" },
            new() { Id = 3, Name = "Duitsland" },
            new() { Id = 4, Name = "Frankrijk" }
        };

        await _countryRepository.AddCountriesAsync(countries);
    }

    private async Task SeedCitiesAsync()
    {
        var netherlands = await _countryRepository.GetCountryByIdAsync(1);
        var belgium = await _countryRepository.GetCountryByIdAsync(2);
        var germany = await _countryRepository.GetCountryByIdAsync(3);
        var france = await _countryRepository.GetCountryByIdAsync(4);

        var cities = new List<City>
        {
            new("Amsterdam", 872757, 1),
            new("Rotterdam", 651446, 1),
            new("Den Haag", 548320, 1),
            new("Utrecht", 361966, 1),
            new("Antwerpen", 530504, 2),
            new("Brussel", 1208542, 2),
            new("Gent", 263614, 2),
            new("Berlijn", 3669491, 3),
            new("München", 1488202, 3),
            new("Parijs", 2161000, 4),
            new("Lyon", 518635, 4)
        };

        await _cityRepository.AddCitiesAsync(cities);
    }
}
