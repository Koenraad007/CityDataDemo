using AP.CityDataDemo.Application.DTOs;

namespace AP.CityDataDemo.Application.Services;

public interface ICityService
{
    Task<IEnumerable<CityDto>> GetAllCitiesAsync();
    Task<IEnumerable<CityDto>> GetCitiesSortedByPopulationAsync(bool descending = false);
}
