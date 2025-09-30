using AP.CityDataDemo.Domain;

namespace AP.CityDataDemo.Application.Interfaces;

public interface ICityRepository : IGenericRepository<City>
{
    Task<IEnumerable<City>> GetAllAsync(bool sortByName, bool descending, CancellationToken cancellationToken = default);
    Task<City?> GetCityByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddCityAsync(City city, CancellationToken cancellationToken = default);
    Task AddCitiesAsync(IEnumerable<City> cities, CancellationToken cancellationToken = default);
    Task UpdateCityAsync(City city, CancellationToken cancellationToken = default);
    Task DeleteCityAsync(City city, CancellationToken cancellationToken = default);
    Task<bool> DeleteCityByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> CityNameExistsAsync(string name, CancellationToken cancellationToken = default);
    Task<int> GetCountAsync(CancellationToken cancellationToken = default);
}
