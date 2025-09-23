using AP.CityDataDemo.Application.DTOs;
using AP.CityDataDemo.Application.Mappings;
using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Domain.Entities;

namespace AP.CityDataDemo.Application.Services;

public class CityService : ICityService
{
    private readonly ICityRepository _cityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CityService(ICityRepository cityRepository, IUnitOfWork unitOfWork)
    {
        _cityRepository = cityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CityDto>> GetAllCitiesAsync()
    {
        var cities = await _cityRepository.GetAllAsync();
        return cities.Select(c => c.ToDto());
    }

    public async Task<IEnumerable<CityDto>> GetCitiesSortedByPopulationAsync(bool descending = false)
    {
        var cities = await _cityRepository.GetAllAsync(sortByName: false, descending);
        return cities.Select(c => c.ToDto());
    }

    public async Task<CityDto?> GetCityByIdAsync(int id)
    {
        var city = await _cityRepository.GetCityByIdAsync(id);
        return city?.ToDto();
    }

    public async Task<CityDto> CreateCityAsync(CityDto cityDto)
    {
        var city = cityDto.ToEntity();
        await _cityRepository.AddCityAsync(city);
        await _unitOfWork.SaveChangesAsync();
        return city.ToDto();
    }

    public async Task<CityDto> UpdateCityAsync(int id, CityDto cityDto)
    {
        var city = cityDto.ToEntity();
        city.Id = id;
        await _cityRepository.UpdateCityAsync(city);
        await _unitOfWork.SaveChangesAsync();
        return city.ToDto();
    }

    public async Task DeleteCityAsync(int id)
    {
        await _cityRepository.DeleteCityByIdAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }
}
