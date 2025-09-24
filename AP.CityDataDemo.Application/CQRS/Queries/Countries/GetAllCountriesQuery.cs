using MediatR;
using AP.CityDataDemo.Application.DTOs;

namespace AP.CityDataDemo.Application.CQRS.Queries.Countries;

public record GetAllCountriesQuery : IRequest<IEnumerable<CountryDto>>;
