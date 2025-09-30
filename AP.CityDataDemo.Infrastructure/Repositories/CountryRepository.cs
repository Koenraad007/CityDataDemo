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

    public Task<IEnumerable<Country>> GetAllCountriesAsync(CancellationToken cancellationToken = default)
    {
        return GetAllAsync(cancellationToken);
    }

    public Task<Country?> GetCountryByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return GetByIdAsync(id, cancellationToken);
    }

    public Task AddCountryAsync(Country country, CancellationToken cancellationToken = default)
    {
        return AddAsync(country, cancellationToken);
    }

    public Task AddCountriesAsync(IEnumerable<Country> countries, CancellationToken cancellationToken = default)
    {
        return AddRangeAsync(countries, cancellationToken);
    }

    public Task UpdateCountryAsync(Country country, CancellationToken cancellationToken = default)
    {
        return UpdateAsync(country, cancellationToken);
    }

    public Task DeleteCountryAsync(Country country, CancellationToken cancellationToken = default)
    {
        return DeleteAsync(country, cancellationToken);
    }

    public Task<bool> DeleteCountryByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return DeleteByIdAsync(id, cancellationToken);
    }
}
