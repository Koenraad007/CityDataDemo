using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Domain;
using AP.CityDataDemo.Shared.DTO;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AP.CityDataDemo.Application.CQRS.Queries.Countries
{
    public class GetAllCountriesQuery : IRequest<IEnumerable<CountryDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetAllCountriesQueryHandler : IRequestHandler<GetAllCountriesQuery, IEnumerable<CountryDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetAllCountriesQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CountryDto>> Handle(GetAllCountriesQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<CountryDto>>(
                await _uow.CountriesRepository.GetAllAsync(
                    request.PageNumber,
                    request.PageSize,
                    c => c.Name,
                    true,
                    null,
                    cancellationToken
                )
            );
        }
    }
}