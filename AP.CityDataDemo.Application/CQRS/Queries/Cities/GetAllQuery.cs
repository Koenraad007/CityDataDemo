using AP.CityDataDemo.Application.Interfaces;
using MediatR;
using AP.CityDataDemo.Shared.DTO;
using AutoMapper;

namespace AP.CityDataDemo.Application.CQRS.Queries.Cities
{
    public class GetAllCitiesQuery : IRequest<IEnumerable<CityDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
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
            var cities = await uow.CitiesRepository.GetAllAsync(request.PageNumber, request.PageSize, c => c.Name, true, cancellationToken);
            var countries = await uow.CountriesRepository.GetAllAsync(request.PageNumber, request.PageSize, c => c.Name, true, cancellationToken);
            var countryDict = countries.ToDictionary(c => c.Id, c => c.Name);
            return cities.Select(city =>
            {
                var dto = _mapper.Map<CityDto>(city);
                dto.CountryName = countryDict.TryGetValue(city.CountryId, out var name) ? name : "N/A";
                return dto;
            });
        }
    }

}
