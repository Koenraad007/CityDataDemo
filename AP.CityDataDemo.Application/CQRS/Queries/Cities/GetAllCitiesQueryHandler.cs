using MediatR;
using AP.CityDataDemo.Application.DTOs;
using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Application.Mappings;

namespace AP.CityDataDemo.Application.CQRS.Queries.Cities;

public class GetAllCitiesQueryHandler : IRequestHandler<GetAllCitiesQuery, IEnumerable<CityDto>>
{
    private readonly ICityRepository _cityRepository;
    private readonly ICountryRepository _countryRepository;

    public GetAllCitiesQueryHandler(ICityRepository cityRepository, ICountryRepository countryRepository)
    {
        _cityRepository = cityRepository;
        _countryRepository = countryRepository;
    }

    public async Task<IEnumerable<CityDto>> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken)
    {
        var cities = await _cityRepository.GetAllAsync(sortByName: true, descending: false);
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
