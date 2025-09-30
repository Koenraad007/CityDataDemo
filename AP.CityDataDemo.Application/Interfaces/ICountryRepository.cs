using AP.CityDataDemo.Domain;

namespace AP.CityDataDemo.Application.Interfaces;

public interface ICountryRepository : IGenericRepository<Country>
{
    Task<IEnumerable<Country>> GetAllCountriesAsync();
    Task<Country?> GetCountryByIdAsync(int id);
    Task AddCountryAsync(Country country);
    Task AddCountriesAsync(IEnumerable<Country> countries);
    Task<bool> UpdateCountryAsync(Country country);
    Task DeleteCountryAsync(Country country);
    Task<bool> DeleteCountryByIdAsync(int id);
}
