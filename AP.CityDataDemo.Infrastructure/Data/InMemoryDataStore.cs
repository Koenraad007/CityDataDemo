using AP.CityDataDemo.Domain.Entities;

namespace AP.CityDataDemo.Infrastructure.Data;

public interface IInMemoryDataStore
{
    List<Country> Countries { get; }
    List<City> Cities { get; }
}

public class InMemoryDataStore : IInMemoryDataStore
{
    public List<Country> Countries { get; } = new();
    public List<City> Cities { get; } = new();
}
