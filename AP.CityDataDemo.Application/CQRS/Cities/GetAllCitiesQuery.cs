using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Domain;
using MediatR;

namespace AP.MyGameStore.Application.CQRS.Cities
{
    public class GetAllCitiesQuery : IRequest<IEnumerable<City>>
    {
        public int PageNr { get; set; }
        public int PageSize { get; set; }
    }
    public class GetAllCitiesQueryHandler : IRequestHandler<GetAllCitiesQuery, IEnumerable<City>>
    {
        private readonly IUnitOfWork uow;

        public GetAllCitiesQueryHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        public async Task<IEnumerable<City>> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken)
        {
            return await uow.CitiesRepository.GetAll(request.PageNr, request.PageSize);
        }
    }

}
