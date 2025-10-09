using AP.CityDataDemo.Application.Interfaces;
using MediatR;
using AP.CityDataDemo.Shared.DTO;
using AutoMapper;
using System.Linq.Expressions;

namespace AP.CityDataDemo.Application.CQRS.Queries.Cities
{
    public class GetAllCitiesQuery : IRequest<IEnumerable<CityDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public bool Ascending { get; set; } = true;
        public string? OrderBy { get; set; } = "Name";
    }
    public class GetAllCitiesQueryHandler : IRequestHandler<GetAllCitiesQuery, IEnumerable<CityDto>>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper _mapper;

        public GetAllCitiesQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CityDto>> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken)
        {
            var orderBy = string.IsNullOrWhiteSpace(request.OrderBy) ? "Name" : request.OrderBy;

            Expression<Func<Domain.City, object>> orderByExpr = orderBy.ToLowerInvariant() switch
            {
                "name" => c => c.Name,
                "population" => c => c.Population,
                "country" => c => c.Country.Name,
                _ => c => c.Name // fallback
            };

            var cities = await uow.CitiesRepository.GetAllAsync<object>(
                request.PageNumber,
                request.PageSize,
                orderByExpr,
                request.Ascending,
                new[] { "Country" },
                cancellationToken
            );

            return _mapper.Map<IEnumerable<CityDto>>(cities);
        }
    }

}
