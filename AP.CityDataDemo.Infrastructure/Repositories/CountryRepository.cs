using AP.CityDataDemo.Domain.Entities;
using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Infrastructure.Data;

namespace AP.CityDataDemo.Infrastructure.Repositories;

public class CountryRepository : GenericRepository<Country>, ICountryRepository
{
    private new readonly IInMemoryDataStore _dataStore;
    
    public CountryRepository(IInMemoryDataStore dataStore) : base(dataStore)
    {
        _dataStore = dataStore;
    }

    public Task<IEnumerable<Country>> GetAllCountriesAsync()
    {
        return GetAllAsync();
    }

    public Task<Country?> GetCountryByIdAsync(int id)
    {
        return GetByIdAsync(id);
    }

    public Task AddCountryAsync(Country country)
    {
        if (country.Id == 0)
        {
            country.Id = _dataStore.GetNextCountryId();
        }
        return AddAsync(country);
    }

    public async Task AddCountriesAsync(IEnumerable<Country> countries)
    {
        foreach (var country in countries)
        {
            await AddCountryAsync(country);
        }
    }

    public Task<bool> UpdateCountryAsync(Country country)
    {
        return UpdateAsync(country);
    }

    public Task DeleteCountryAsync(Country country)
    {
        return DeleteAsync(country);
    }

    public Task<bool> DeleteCountryByIdAsync(int id)
    {
        return DeleteByIdAsync(id);
    }
}
