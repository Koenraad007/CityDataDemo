using MediatR;
using AP.CityDataDemo.Application.DTOs;

namespace AP.CityDataDemo.Application.CQRS.Commands.Cities;

public record CreateCityCommand(AddCityDto AddCityDto) : IRequest<CityDto>;
