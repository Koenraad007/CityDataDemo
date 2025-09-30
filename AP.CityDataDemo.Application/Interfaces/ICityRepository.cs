using AP.CityDataDemo.Domain;

namespace AP.CityDataDemo.Application.Interfaces;

public interface ICityRepository : IGenericRepository<City>
{
    Task<IEnumerable<City>> GetAllAsync(bool sortByName, bool descending);
    Task<City?> GetCityByIdAsync(int id);
    Task AddCityAsync(City city);
    Task AddCitiesAsync(IEnumerable<City> cities);
    Task<bool> UpdateCityAsync(City city);
    Task DeleteCityAsync(City city);
    Task<bool> DeleteCityByIdAsync(int id);
    Task<bool> CityNameExistsAsync(string name);
    Task<int> GetCountAsync();
}
