using AP.CityDataDemo.Domain;
using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AP.CityDataDemo.Infrastructure.Repositories;

public class CountryRepository : GenericRepository<Country>, ICountryRepository
{
    public CountryRepository(CityDataDemoContext cityDataDemoContext) : base(cityDataDemoContext)
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
