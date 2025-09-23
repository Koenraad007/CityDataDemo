using AP.CityDataDemo.Application.DTOs;
using AP.CityDataDemo.Application.Mappings;
using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Domain.Entities;

namespace AP.CityDataDemo.Application.Services;

public class CityService : ICityService
{
    private readonly ICityRepository _cityRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CityService(ICityRepository cityRepository, ICountryRepository countryRepository, IUnitOfWork unitOfWork)
    {
        _cityRepository = cityRepository;
        _countryRepository = countryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CityDto>> GetAllCitiesAsync()
    {
        var cities = await _cityRepository.GetAllAsync(sortByName: true, descending: false);
        var countries = await _countryRepository.GetAllCountriesAsync();
        var countryMap = countries.ToDictionary(c => c.Id, c => c.Name);
        
        return cities.Select(c => new CityDto
        {
            Id = c.Id,
            Name = c.Name,
            Population = c.Population,
            CountryName = countryMap.TryGetValue(c.CountryId, out var countryName) ? countryName : "N/A"
        });
    }

    public async Task<IEnumerable<CityDto>> GetCitiesSortedByPopulationAsync(bool descending = false)
    {
        var cities = await _cityRepository.GetAllAsync(sortByName: false, descending);
        var countries = await _countryRepository.GetAllCountriesAsync();
        var countryMap = countries.ToDictionary(c => c.Id, c => c.Name);
        
        return cities.Select(c => new CityDto
        {
            Id = c.Id,
            Name = c.Name,
            Population = c.Population,
            CountryName = countryMap.TryGetValue(c.CountryId, out var countryName) ? countryName : "N/A"
        });
    }

    public async Task<CityDto?> GetCityByIdAsync(int id)
    {
        var city = await _cityRepository.GetCityByIdAsync(id);
        if (city == null) return null;
        
        var country = await _countryRepository.GetCountryByIdAsync(city.CountryId);
        return new CityDto
        {
            Id = city.Id,
            Name = city.Name,
            Population = city.Population,
            CountryName = country?.Name ?? "N/A"
        };
    }

    public async Task<CityDto> CreateCityAsync(CityDto cityDto)
    {
        var city = cityDto.ToEntity();
        await _cityRepository.AddCityAsync(city);
        await _unitOfWork.SaveChangesAsync();
        
        var country = await _countryRepository.GetCountryByIdAsync(city.CountryId);
        return new CityDto
        {
            Id = city.Id,
            Name = city.Name,
            Population = city.Population,
            CountryName = country?.Name ?? "N/A"
        };
    }

    public async Task<CityDto> UpdateCityAsync(int id, CityDto cityDto)
    {
        var city = cityDto.ToEntity();
        city.Id = id;
        await _cityRepository.UpdateCityAsync(city);
        await _unitOfWork.SaveChangesAsync();
        
        var country = await _countryRepository.GetCountryByIdAsync(city.CountryId);
        return new CityDto
        {
            Id = city.Id,
            Name = city.Name,
            Population = city.Population,
            CountryName = country?.Name ?? "N/A"
        };
    }

    public async Task DeleteCityAsync(int id)
    {
        await _cityRepository.DeleteCityByIdAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }
}
