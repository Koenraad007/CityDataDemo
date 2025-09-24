using AP.CityDataDemo.Application.DTOs;
using AP.CityDataDemo.Application.Mappings;
using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Application.Exceptions;
using AP.CityDataDemo.Domain.Entities;
using FluentValidation;

namespace AP.CityDataDemo.Application.Services;

public class CityService : ICityService
{
    private readonly ICityRepository _cityRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<AddCityDto> _addCityValidator;
    private readonly IValidator<CityDto> _cityValidator;

    public CityService(
        ICityRepository cityRepository, 
        ICountryRepository countryRepository, 
        IUnitOfWork unitOfWork,
        IValidator<AddCityDto> addCityValidator,
        IValidator<CityDto> cityValidator)
    {
        _cityRepository = cityRepository;
        _countryRepository = countryRepository;
        _unitOfWork = unitOfWork;
        _addCityValidator = addCityValidator;
        _cityValidator = cityValidator;
    }

    public async Task<IEnumerable<CityDto>> GetAllCitiesAsync()
    {
        var cities = await _cityRepository.GetAllAsync(sortByName: true, descending: false);
        return await MapCitiesToDtosAsync(cities);
    }

    public async Task<IEnumerable<CityDto>> GetCitiesSortedByPopulationAsync(bool descending = false)
    {
        var cities = await _cityRepository.GetAllAsync(sortByName: false, descending);
        return await MapCitiesToDtosAsync(cities);
    }

    public async Task<CityDto?> GetCityByIdAsync(int id)
    {
        var city = await _cityRepository.GetCityByIdAsync(id);
        if (city == null) return null;
        
        var country = await _countryRepository.GetCountryByIdAsync(city.CountryId);
        var cityDto = city.ToDto();
        cityDto.CountryName = country?.Name ?? "N/A";
        return cityDto;
    }

    public async Task<CityDto> CreateCityAsync(CityDto cityDto)
    {
        var city = cityDto.ToEntity();
        await _cityRepository.AddCityAsync(city);
        await _unitOfWork.SaveChangesAsync();
        
        var country = await _countryRepository.GetCountryByIdAsync(city.CountryId);
        var resultDto = city.ToDto();
        resultDto.CountryName = country?.Name ?? "N/A";
        return resultDto;
    }

    public async Task<CityDto> UpdateCityAsync(int id, CityDto cityDto)
    {
        await _cityValidator.ValidateAndThrowAsync(cityDto);
        
        var city = cityDto.ToEntity();
        city.Id = id;
        
        var success = await _cityRepository.UpdateCityAsync(city);
        if (!success)
        {
            throw new NotFoundException($"Stad met id {id} werd niet gevonden");
        }
        
        await _unitOfWork.SaveChangesAsync();
        
        var country = await _countryRepository.GetCountryByIdAsync(city.CountryId);
        var resultDto = city.ToDto();
        resultDto.CountryName = country?.Name ?? "N/A";
        return resultDto;
    }

    public async Task DeleteCityAsync(int id)
    {
        var success = await _cityRepository.DeleteCityByIdAsync(id);
        if (!success)
        {
            throw new NotFoundException($"Stad met id {id} werd niet gevonden");
        }
        
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<CityDto> CreateCityAsync(AddCityDto addCityDto)
    {
        await _addCityValidator.ValidateAndThrowAsync(addCityDto);

        var city = new City(addCityDto.Name, addCityDto.Population, addCityDto.CountryId);
        await _cityRepository.AddCityAsync(city);
        await _unitOfWork.SaveChangesAsync();

        var country = await _countryRepository.GetCountryByIdAsync(addCityDto.CountryId);
        var resultDto = city.ToDto();
        resultDto.CountryName = country?.Name ?? "N/A";
        return resultDto;
    }

    public async Task<IEnumerable<CountryDto>> GetAllCountriesAsync()
    {
        var countries = await _countryRepository.GetAllCountriesAsync();
        return countries.Select(c => new CountryDto
        {
            Id = c.Id,
            Name = c.Name
        });
    }

    private async Task<IEnumerable<CityDto>> MapCitiesToDtosAsync(IEnumerable<City> cities)
    {
        var countries = await _countryRepository.GetAllCountriesAsync();
        var countryMap = countries.ToDictionary(c => c.Id, c => c.Name);
        
        return cities.Select(c =>
        {
            var dto = c.ToDto();
            dto.CountryName = countryMap.TryGetValue(c.CountryId, out var countryName) ? countryName : "N/A";
            return dto;
        });
    }
}
