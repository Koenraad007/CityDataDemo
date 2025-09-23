using AP.CityDataDemo.Application.DTOs;
using AP.CityDataDemo.Domain.Entities;
using AP.CityDataDemo.Domain.Interfaces;

namespace AP.CityDataDemo.Application.Services;

public class CityService : GenericService<CityDto, City>, ICityService
{
    private readonly ICityRepository _cityRepository;

    public CityService(IUnitOfWork unitOfWork, ICityRepository cityRepository) 
        : base(unitOfWork, cityRepository)
    {
        _cityRepository = cityRepository;
    }

    public async Task<IEnumerable<CityDto>> GetAllCitiesAsync()
    {
        return await GetAllAsync();
    }

    public async Task<IEnumerable<CityDto>> GetCitiesSortedByPopulationAsync(bool descending = false)
    {
        var cities = await GetAllCitiesAsync();
        return descending 
            ? cities.OrderByDescending(c => c.Population)
            : cities.OrderBy(c => c.Population);
    }

    protected override CityDto MapToDto(City entity)
    {
        return new CityDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Population = entity.Population,
            CountryName = entity.Country.Name
        };
    }

    protected override City MapToEntity(CityDto dto)
    {
        return new City
        {
            Id = dto.Id,
            Name = dto.Name,
            Population = dto.Population
        };
    }
}
