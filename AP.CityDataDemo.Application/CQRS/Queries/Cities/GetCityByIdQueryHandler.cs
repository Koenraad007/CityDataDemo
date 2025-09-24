using MediatR;
using AP.CityDataDemo.Application.DTOs;
using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Application.Mappings;

namespace AP.CityDataDemo.Application.CQRS.Queries.Cities;

public class GetCityByIdQueryHandler : IRequestHandler<GetCityByIdQuery, CityDto?>
{
    private readonly ICityRepository _cityRepository;
    private readonly ICountryRepository _countryRepository;

    public GetCityByIdQueryHandler(ICityRepository cityRepository, ICountryRepository countryRepository)
    {
        _cityRepository = cityRepository;
        _countryRepository = countryRepository;
    }

    public async Task<CityDto?> Handle(GetCityByIdQuery request, CancellationToken cancellationToken)
    {
        var city = await _cityRepository.GetCityByIdAsync(request.Id);
        if (city == null) 
            return null;
        
        var country = await _countryRepository.GetCountryByIdAsync(city.CountryId);
        var cityDto = city.ToDto();
        cityDto.CountryName = country?.Name ?? "N/A";
        return cityDto;
    }
}
