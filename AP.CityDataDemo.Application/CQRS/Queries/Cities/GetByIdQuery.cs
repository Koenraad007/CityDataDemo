using MediatR;
using AP.CityDataDemo.Shared.DTO;
using AP.CityDataDemo.Application.Interfaces;
using AutoMapper;

namespace AP.CityDataDemo.Application.CQRS.Queries.Cities;

public record GetCityByIdQuery(int Id) : IRequest<CityDto?>;

public class GetCityByIdQueryHandler : IRequestHandler<GetCityByIdQuery, CityDto?>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetCityByIdQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;

    }

    public async Task<CityDto?> Handle(GetCityByIdQuery request, CancellationToken cancellationToken)
    {
        var city = await _uow.CitiesRepository.GetByIdAsync(request.Id);
        if (city == null)
            return null;

        var country = await _uow.CountriesRepository.GetCountryByIdAsync(city.CountryId);
        var cityDto = _mapper.Map<CityDto>(city);
        cityDto.CountryName = country?.Name ?? "N/A";
        return cityDto;
    }
}
