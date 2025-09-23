using AP.CityDataDemo.Domain.Entities;

namespace AP.CityDataDemo.Domain.Interfaces;

public interface ICityRepository : IGenericRepository<City>
{
    Task<IEnumerable<City>> GetAllCitiesAsync();
    Task<City?> GetCityByIdAsync(int id);
    Task AddCityAsync(City city);
    Task AddCitiesAsync(IEnumerable<City> cities);
}
