using MediatR;
using AP.CityDataDemo.Shared.DTO;
using AP.CityDataDemo.Application.Interfaces;
using AutoMapper;

namespace AP.CityDataDemo.Application.CQRS.Queries.Cities;

public record GetCitiesSortedByPopulationQuery(bool Descending = false) : IRequest<IEnumerable<CityDto>>;

public class GetCitiesSortedByPopulationQueryHandler : IRequestHandler<GetCitiesSortedByPopulationQuery, IEnumerable<CityDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetCitiesSortedByPopulationQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CityDto>> Handle(GetCitiesSortedByPopulationQuery request, CancellationToken cancellationToken)
    {
        var cities = await _uow.CitiesRepository.GetAllAsync(sortByName: false, descending: request.Descending);
        var countries = await _uow.CountriesRepository.GetAllAsync();
        var countryMap = countries.ToDictionary(c => c.Id, c => c.Name);

        return cities.Select(c =>
        {
            var dto = _mapper.Map<CityDto>(c);
            dto.CountryName = countryMap.TryGetValue(c.CountryId, out var countryName) ? countryName : "N/A";
            return dto;
        });
    }
}
