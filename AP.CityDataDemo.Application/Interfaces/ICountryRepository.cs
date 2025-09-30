using AP.CityDataDemo.Domain;

namespace AP.CityDataDemo.Application.Interfaces;

public interface ICountryRepository : IGenericRepository<Country>
{
    Task<IEnumerable<Country>> GetAllCountriesAsync(CancellationToken cancellationToken = default);
    Task<Country?> GetCountryByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddCountryAsync(Country country, CancellationToken cancellationToken = default);
    Task AddCountriesAsync(IEnumerable<Country> countries, CancellationToken cancellationToken = default);
    Task UpdateCountryAsync(Country country, CancellationToken cancellationToken = default);
    Task DeleteCountryAsync(Country country, CancellationToken cancellationToken = default);
    Task<bool> DeleteCountryByIdAsync(int id, CancellationToken cancellationToken = default);
}
