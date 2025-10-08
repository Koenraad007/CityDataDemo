using MediatR;
using AP.CityDataDemo.Shared.DTO;
using AP.CityDataDemo.Application.Interfaces;

namespace AP.CityDataDemo.Application.CQRS.Queries.Cities;

public record GetCityByIdQuery(int Id) : IRequest<CityDto?>;

public class GetCityByIdQueryHandler : IRequestHandler<GetCityByIdQuery, CityDto?>
{
    private readonly IUnitOfWork _uow;

    public GetCityByIdQueryHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<CityDto?> Handle(GetCityByIdQuery request, CancellationToken cancellationToken)
    {
        var city = await _uow.CitiesRepository.GetByIdAsync(request.Id);
        if (city == null)
            return null;

        var country = await _uow.CountriesRepository.GetCountryByIdAsync(city.CountryId);
        var cityDto = CityMapper.ToDto(city);
        cityDto.CountryName = country?.Name ?? "N/A";
        return cityDto;
    }
}
