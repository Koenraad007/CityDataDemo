using MediatR;
using AP.CityDataDemo.Shared.DTO;
using AP.CityDataDemo.Application.Interfaces;
using FluentValidation;
using AP.CityDataDemo.Domain;
using AutoMapper;

namespace AP.CityDataDemo.Application.CQRS.Commands.Cities;

public record CreateCityCommand(AddCityDto AddCityDto) : IRequest<CityDto>;

public class CreateCityCommandValidator : AbstractValidator<CreateCityCommand>
{
    private readonly ICityRepository _cityRepository;
    private readonly ICountryRepository _countryRepository;

    public CreateCityCommandValidator(ICityRepository cityRepository, ICountryRepository countryRepository)
    {
        _cityRepository = cityRepository;
        _countryRepository = countryRepository;

        RuleFor(x => x.AddCityDto.Name)
            .NotEmpty()
            .WithMessage("Name cannot be empty")
            .MaximumLength(100)
            .WithMessage("Name cannot be longer than 100 characters")
            .MustAsync(async (name, cancellation) => !await _cityRepository.CityNameExistsAsync(name))
            .WithMessage("Name already exists");

        RuleFor(x => x.AddCityDto.Population)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Population cannot be negative")
            .LessThanOrEqualTo(10000000000)
            .WithMessage("Population cannot be greater than 10,000,000,000");

        RuleFor(x => x.AddCityDto.CountryId)
            .GreaterThan(0)
            .WithMessage("A country must be selected")
            .MustAsync(async (countryId, cancellation) => await _countryRepository.GetCountryByIdAsync(countryId) != null)
            .WithMessage("The selected country does not exist");
    }
}

public class CreateCityCommandHandler : IRequestHandler<CreateCityCommand, CityDto>
{
    private readonly ICityRepository _cityRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCityCommandHandler(
        ICityRepository cityRepository,
        ICountryRepository countryRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _cityRepository = cityRepository;
        _countryRepository = countryRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CityDto> Handle(CreateCityCommand request, CancellationToken cancellationToken)
    {
        var city = new City() { Name = request.AddCityDto.Name, Population = (int)request.AddCityDto.Population, CountryId = request.AddCityDto.CountryId };
        await _cityRepository.AddCityAsync(city, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        var country = await _countryRepository.GetCountryByIdAsync(request.AddCityDto.CountryId, cancellationToken);
        var resultDto = _mapper.Map<CityDto>(city);
        resultDto.CountryName = country?.Name ?? "N/A";
        return resultDto;
    }
}
