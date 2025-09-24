using MediatR;
using AP.CityDataDemo.Application.DTOs;

namespace AP.CityDataDemo.Application.CQRS.Queries.Cities;

public record GetCitiesSortedByPopulationQuery(bool Descending = false) : IRequest<IEnumerable<CityDto>>;
