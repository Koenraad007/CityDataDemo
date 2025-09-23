using AP.CityDataDemo.Domain.Entities;

namespace AP.CityDataDemo.Domain.Interfaces;

public interface ICountryRepository
{
    Task<IEnumerable<Country>> GetAllCountriesAsync();
    Task<Country?> GetCountryByIdAsync(int id);
}
