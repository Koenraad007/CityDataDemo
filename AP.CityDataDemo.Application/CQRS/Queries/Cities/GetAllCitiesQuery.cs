using MediatR;
using AP.CityDataDemo.Application.DTOs;

namespace AP.CityDataDemo.Application.CQRS.Queries.Cities;

public record GetAllCitiesQuery : IRequest<IEnumerable<CityDto>>;
