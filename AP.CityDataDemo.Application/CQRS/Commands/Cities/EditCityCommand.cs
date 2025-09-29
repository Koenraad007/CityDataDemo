using MediatR;
using AP.CityDataDemo.Domain;
using AP.CityDataDemo.Application.Interfaces;

namespace AP.CityDataDemo.Application.CQRS.Commands.Cities
{
    public class EditCityCommand : IRequest<City>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Population { get; set; }
        public int CountryId { get; set; }

        public EditCityCommand(int id, string name, int populationn, int countryId)
        {
            Id = id;
            Name = name;
            Population = populationn;
            CountryId = countryId;
        }
    }

    public class EditCityCommandHandler : IRequestHandler<EditCityCommand, City>
    {
        private readonly IUnitOfWork unitOfWork;

        public EditCityCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<City> Handle(EditCityCommand request, CancellationToken cancellationToken)
        {
            var city = await unitOfWork.CitiesRepository.GetById(request.Id);
            if (city == null)
            {
                return null;
            }

            city.Name = request.Name;
            city.Population = request.Population;
            city.CountryId = request.CountryId;

            unitOfWork.CitiesRepository.Update(city);
            await unitOfWork.Commit();

            return city;
        }
    }
}
