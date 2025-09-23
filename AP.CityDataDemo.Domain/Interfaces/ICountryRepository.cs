using AP.CityDataDemo.Domain.Entities;

namespace AP.CityDataDemo.Domain.Interfaces;

public interface ICountryRepository : IGenericRepository<Country>
{
    Task<IEnumerable<Country>> GetAllCountriesAsync();
    Task<Country?> GetCountryByIdAsync(int id);
    Task AddCountryAsync(Country country);
    Task AddCountriesAsync(IEnumerable<Country> countries);
}
