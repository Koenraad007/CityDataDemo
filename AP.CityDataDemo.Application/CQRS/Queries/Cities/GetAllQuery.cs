using AP.CityDataDemo.Application.Interfaces;
using MediatR;
using AP.CityDataDemo.Application.DTOs;
using AP.CityDataDemo.Application.Mappings;

namespace AP.MyGameStore.Application.CQRS.Queries.Cities
{
    public class GetAllCitiesQuery : IRequest<IEnumerable<CityDto>>
    { }
    public class GetAllCitiesQueryHandler : IRequestHandler<GetAllCitiesQuery, IEnumerable<CityDto>>
    {
        private readonly IUnitOfWork uow;

        public GetAllCitiesQueryHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        public async Task<IEnumerable<CityDto>> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken)
        {
            var cities = await uow.CitiesRepository.GetAllAsync();
            var countries = await uow.CountriesRepository.GetAllAsync();
            var countryDict = countries.ToDictionary(c => c.Id, c => c.Name);
            return cities.Select(city =>
            {
                var dto = CityMapper.ToDto(city);
                dto.CountryName = countryDict.TryGetValue(city.CountryId, out var name) ? name : "N/A";
                return dto;
            });
        }
    }

}
