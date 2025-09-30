using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Domain;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AP.CityDataDemo.Application.CQRS.Queries.Countries
{
    public class GetAllCountriesQuery : IRequest<IEnumerable<Country>>
    { }

    public class GetAllCountriesQueryHandler : IRequestHandler<GetAllCountriesQuery, IEnumerable<Country>>
    {
        private readonly IUnitOfWork _uow;

        public GetAllCountriesQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<Country>> Handle(GetAllCountriesQuery request, CancellationToken cancellationToken)
        {
            return await _uow.CountriesRepository.GetAllAsync();
        }
    }
}