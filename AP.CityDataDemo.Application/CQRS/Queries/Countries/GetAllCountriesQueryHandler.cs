using MediatR;
using AP.CityDataDemo.Application.DTOs;
using AP.CityDataDemo.Application.Interfaces;

namespace AP.CityDataDemo.Application.CQRS.Queries.Countries;

public class GetAllCountriesQueryHandler : IRequestHandler<GetAllCountriesQuery, IEnumerable<CountryDto>>
{
    private readonly ICountryRepository _countryRepository;

    public GetAllCountriesQueryHandler(ICountryRepository countryRepository)
    {
        _countryRepository = countryRepository;
    }

    public async Task<IEnumerable<CountryDto>> Handle(GetAllCountriesQuery request, CancellationToken cancellationToken)
    {
        var countries = await _countryRepository.GetAllCountriesAsync();
        return countries.Select(c => new CountryDto
        {
            Id = c.Id,
            Name = c.Name
        });
    }
}
