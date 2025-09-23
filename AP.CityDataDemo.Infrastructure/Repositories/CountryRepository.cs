using AP.CityDataDemo.Domain.Entities;
using AP.CityDataDemo.Domain.Interfaces;
using AP.CityDataDemo.Infrastructure.Data;

namespace AP.CityDataDemo.Infrastructure.Repositories;

public class CountryRepository : ICountryRepository
{
    private readonly IInMemoryDataStore _dataStore;

    public CountryRepository(IInMemoryDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public Task<IEnumerable<Country>> GetAllCountriesAsync()
    {
        return Task.FromResult<IEnumerable<Country>>(_dataStore.Countries);
    }

    public Task<Country?> GetCountryByIdAsync(int id)
    {
        var country = _dataStore.Countries.FirstOrDefault(c => c.Id == id);
        return Task.FromResult(country);
    }

    public Task AddCountryAsync(Country country)
    {
        _dataStore.Countries.Add(country);
        return Task.CompletedTask;
    }

    public Task AddCountriesAsync(IEnumerable<Country> countries)
    {
        _dataStore.Countries.AddRange(countries);
        return Task.CompletedTask;
    }
}
