using AP.CityDataDemo.Domain.Entities;
using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Infrastructure.Data;

namespace AP.CityDataDemo.Infrastructure.Repositories;

public class CountryRepository : GenericRepository<Country>, ICountryRepository
{
    public CountryRepository(IInMemoryDataStore dataStore) : base(dataStore)
    {
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
        return AddAsync(country);
    }

    public Task AddCountriesAsync(IEnumerable<Country> countries)
    {
        return AddRangeAsync(countries);
    }

    public Task UpdateCountryAsync(Country country)
    {
        return UpdateAsync(country);
    }

    public Task DeleteCountryAsync(Country country)
    {
        return DeleteAsync(country);
    }

    public Task DeleteCountryByIdAsync(int id)
    {
        return DeleteByIdAsync(id);
    }
}
