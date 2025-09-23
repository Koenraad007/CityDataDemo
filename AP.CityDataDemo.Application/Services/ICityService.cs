using AP.CityDataDemo.Application.DTOs;
using AP.CityDataDemo.Domain.Entities;

namespace AP.CityDataDemo.Application.Services;

public interface ICityService : IGenericService<CityDto, City>
{
    Task<IEnumerable<CityDto>> GetAllCitiesAsync();
    Task<IEnumerable<CityDto>> GetCitiesSortedByPopulationAsync(bool descending = false);
}
