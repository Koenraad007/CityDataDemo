using AP.CityDataDemo.Domain.Entities;
using AP.CityDataDemo.Domain.Interfaces;
using AP.CityDataDemo.Infrastructure.Data;

namespace AP.CityDataDemo.Infrastructure.Repositories;

public class CityRepository : GenericRepository<City>, ICityRepository
{
    public CityRepository(IInMemoryDataStore dataStore) : base(dataStore)
    {
    }

    public Task<IEnumerable<City>> GetAllCitiesAsync()
    {
        return GetAllAsync();
    }

    public Task<City?> GetCityByIdAsync(int id)
    {
        return GetByIdAsync(id);
    }

    public Task AddCityAsync(City city)
    {
        return AddAsync(city);
    }

    public Task AddCitiesAsync(IEnumerable<City> cities)
    {
        return AddRangeAsync(cities);
    }
}
