using AP.CityDataDemo.Application.DTOs;
using AP.CityDataDemo.Domain.Interfaces;

namespace AP.CityDataDemo.Application.Services;

public class CityService : ICityService
{
    private readonly ICityRepository _cityRepository;

    public CityService(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }

    public async Task<IEnumerable<CityDto>> GetAllCitiesAsync()
    {
        var cities = await _cityRepository.GetAllCitiesAsync();
        return cities.Select(c => new CityDto
        {
            Id = c.Id,
            Name = c.Name,
            Population = c.Population,
            CountryName = c.Country.Name
        });
    }

    public async Task<IEnumerable<CityDto>> GetCitiesSortedByPopulationAsync(bool descending = false)
    {
        var cities = await GetAllCitiesAsync();
        return descending 
            ? cities.OrderByDescending(c => c.Population)
            : cities.OrderBy(c => c.Population);
    }
}
