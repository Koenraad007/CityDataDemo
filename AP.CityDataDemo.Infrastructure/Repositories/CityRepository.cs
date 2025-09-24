using AP.CityDataDemo.Domain.Entities;
using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Infrastructure.Data;

namespace AP.CityDataDemo.Infrastructure.Repositories;

public class CityRepository : GenericRepository<City>, ICityRepository
{
    private new readonly IInMemoryDataStore _dataStore;
    
    public CityRepository(IInMemoryDataStore dataStore) : base(dataStore)
    {
        _dataStore = dataStore;
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
        if (city.Id == 0)
        {
            city.Id = _dataStore.GetNextCityId();
        }
        return AddAsync(city);
    }

    public async Task AddCitiesAsync(IEnumerable<City> cities)
    {
        foreach (var city in cities)
        {
            await AddCityAsync(city);
        }
    }

    public Task<bool> UpdateCityAsync(City city)
    {
        return UpdateAsync(city);
    }

    public Task DeleteCityAsync(City city)
    {
        return DeleteAsync(city);
    }

    public Task<bool> DeleteCityByIdAsync(int id)
    {
        return DeleteByIdAsync(id);
    }

    public Task<bool> CityNameExistsAsync(string name)
    {
        var exists = _collection.Any(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(exists);
    }
}
