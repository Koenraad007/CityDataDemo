using AP.CityDataDemo.Domain;

namespace AP.CityDataDemo.Application.Interfaces;

public interface ICityRepository : IGenericRepository<City>
{
    Task<bool> CityNameExistsAsync(string name, CancellationToken cancellationToken = default);
    Task<int> GetCountAsync(CancellationToken cancellationToken = default);
}
