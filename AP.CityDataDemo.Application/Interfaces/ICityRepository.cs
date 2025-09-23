using AP.CityDataDemo.Domain.Entities;

namespace AP.CityDataDemo.Application.Interfaces;

public interface ICityRepository : IGenericRepository<City>
{
    Task<IEnumerable<City>> GetAllAsync(bool sortByName, bool descending);
    Task<City?> GetCityByIdAsync(int id);
    Task AddCityAsync(City city);
    Task AddCitiesAsync(IEnumerable<City> cities);
    Task UpdateCityAsync(City city);
    Task DeleteCityAsync(City city);
    Task DeleteCityByIdAsync(int id);
}
