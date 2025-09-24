using AP.CityDataDemo.Domain.Entities;

namespace AP.CityDataDemo.Infrastructure.Data;

public interface IInMemoryDataStore
{
    List<Country> Countries { get; }
    List<City> Cities { get; }
    int GetNextCountryId();
    int GetNextCityId();
}

public class InMemoryDataStore : IInMemoryDataStore
{
    private int _nextCountryId = 1;
    private int _nextCityId = 1;
    
    public List<Country> Countries { get; } = new();
    public List<City> Cities { get; } = new();
    
    public int GetNextCountryId() => _nextCountryId++;
    public int GetNextCityId() => _nextCityId++;
}
