using AP.CityDataDemo.Domain.Entities;
using AP.CityDataDemo.Domain.Interfaces;
using AP.CityDataDemo.Infrastructure.Data;

namespace AP.CityDataDemo.Infrastructure.Repositories;

public class CityRepository : ICityRepository
{
    private readonly IInMemoryDataStore _dataStore;

    public CityRepository(IInMemoryDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public Task<IEnumerable<City>> GetAllCitiesAsync()
    {
        return Task.FromResult<IEnumerable<City>>(_dataStore.Cities);
    }

    public Task<City?> GetCityByIdAsync(int id)
    {
        var city = _dataStore.Cities.FirstOrDefault(c => c.Id == id);
        return Task.FromResult(city);
    }
}
