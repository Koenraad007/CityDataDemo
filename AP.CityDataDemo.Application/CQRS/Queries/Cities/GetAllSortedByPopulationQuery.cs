using MediatR;
using AP.CityDataDemo.Application.DTOs;
using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Application.Mappings;

namespace AP.CityDataDemo.Application.CQRS.Queries.Cities;

public record GetCitiesSortedByPopulationQuery(bool Descending = false) : IRequest<IEnumerable<CityDto>>;

public class GetCitiesSortedByPopulationQueryHandler : IRequestHandler<GetCitiesSortedByPopulationQuery, IEnumerable<CityDto>>
{
    private readonly IUnitOfWork _uow;

    public GetCitiesSortedByPopulationQueryHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<IEnumerable<CityDto>> Handle(GetCitiesSortedByPopulationQuery request, CancellationToken cancellationToken)
    {
        var cities = await _uow.CitiesRepository.GetAllAsync(sortByName: false, descending: request.Descending);
        var countries = await _uow.CountriesRepository.GetAllCountriesAsync();
        var countryMap = countries.ToDictionary(c => c.Id, c => c.Name);

        return cities.Select(c =>
        {
            var dto = CityMapper.ToDto(c);
            dto.CountryName = countryMap.TryGetValue(c.CountryId, out var countryName) ? countryName : "N/A";
            return dto;
        });
    }
}
