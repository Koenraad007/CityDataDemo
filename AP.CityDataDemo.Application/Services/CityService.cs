using AP.CityDataDemo.Application.DTOs;
using AP.CityDataDemo.Application.Mappings;
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
        var cities = await _cityRepository.GetAllAsync(sortByName: false, descending);
        return cities.Select(c => c.ToDto());
    }

    protected override CityDto MapToDto(City entity)
    {
        return entity.ToDto();
    }

    protected override City MapToEntity(CityDto dto)
    {
        return dto.ToEntity();
    }
}
