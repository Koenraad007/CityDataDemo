using AP.CityDataDemo.Domain.Entities;
using AP.CityDataDemo.Domain.Interfaces;
using AP.CityDataDemo.Infrastructure.Data;

namespace AP.CityDataDemo.Infrastructure.Repositories;

public class CityRepository : GenericRepository<City>, ICityRepository
{
    public CityRepository(IInMemoryDataStore dataStore) : base(dataStore)
    {
    }

    public Task<IEnumerable<City>> GetAllAsync(bool sortByName, bool descending)
    {
        var cities = _collection.AsEnumerable();
        
        if (sortByName)
        {
            cities = descending 
                ? cities.OrderByDescending(c => c.Name)
                : cities.OrderBy(c => c.Name);
        }
        else
        {
            cities = descending 
                ? cities.OrderByDescending(c => c.Population)
                : cities.OrderBy(c => c.Population);
        }
        
        return Task.FromResult(cities);
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
