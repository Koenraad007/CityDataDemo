using AP.CityDataDemo.Application.DTOs;

namespace AP.CityDataDemo.Application.Services;

public interface ICityService
{
    Task<IEnumerable<CityDto>> GetAllCitiesAsync();
    Task<IEnumerable<CityDto>> GetCitiesSortedByPopulationAsync(bool descending = false);
    Task<CityDto?> GetCityByIdAsync(int id);
    Task<CityDto> CreateCityAsync(CityDto cityDto);
    Task<CityDto> CreateCityAsync(AddCityDto addCityDto);
    Task<CityDto> UpdateCityAsync(int id, CityDto cityDto);
    Task DeleteCityAsync(int id);
    Task<IEnumerable<CountryDto>> GetAllCountriesAsync();
}
