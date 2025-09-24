using MediatR;
using AP.CityDataDemo.Application.DTOs;
using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Application.Mappings;

namespace AP.CityDataDemo.Application.CQRS.Queries.Cities;

public class GetCitiesSortedByPopulationQueryHandler : IRequestHandler<GetCitiesSortedByPopulationQuery, IEnumerable<CityDto>>
{
    private readonly ICityRepository _cityRepository;
    private readonly ICountryRepository _countryRepository;

    public GetCitiesSortedByPopulationQueryHandler(ICityRepository cityRepository, ICountryRepository countryRepository)
    {
        _cityRepository = cityRepository;
        _countryRepository = countryRepository;
    }

    public async Task<IEnumerable<CityDto>> Handle(GetCitiesSortedByPopulationQuery request, CancellationToken cancellationToken)
    {
        var cities = await _cityRepository.GetAllAsync(sortByName: false, descending: request.Descending);
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
