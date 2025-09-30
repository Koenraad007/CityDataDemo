using MediatR;
using AP.CityDataDemo.Domain;
using AP.CityDataDemo.Application.Interfaces;
using FluentValidation;

namespace AP.CityDataDemo.Application.CQRS.Commands.Cities
{
    public class EditCityCommand : IRequest<City>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Population { get; set; }
        public int CountryId { get; set; }

        public EditCityCommand(int id, string name, int population, int countryId)
        {
            Id = id;
            Name = name;
            Population = population;
            CountryId = countryId;
        }
    }

    public class EditCityCommandValidator : AbstractValidator<EditCityCommand>
    {
        private IUnitOfWork unitOfWork;

        public EditCityCommandValidator(IUnitOfWork uow)
        {
            unitOfWork = uow;

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("City name is required.")
                .MaximumLength(100).WithMessage("City name must not exceed 100 characters.")
                .MustAsync(async (command, name, cancellation) =>
                {
                    var city = await unitOfWork.CitiesRepository.GetById(command.Id);
                    return city == null || city.Name == name;
                })
                .WithMessage("City name cannot be changed.");

            RuleFor(c => c.Population)
                .GreaterThanOrEqualTo(0).WithMessage("Population must be a non-negative number.");

            RuleFor(c => c.CountryId)
                .GreaterThan(0).WithMessage("CountryId must be a positive integer.");
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
