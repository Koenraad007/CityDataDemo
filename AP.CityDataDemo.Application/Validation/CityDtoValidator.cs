using FluentValidation;
using AP.CityDataDemo.Application.DTOs;
using AP.CityDataDemo.Application.Interfaces;

namespace AP.CityDataDemo.Application.Validation;

public class CityDtoValidator : AbstractValidator<CityDto>
{
    private readonly ICityRepository _cityRepository;
    private readonly ICountryRepository _countryRepository;

    public CityDtoValidator(ICityRepository cityRepository, ICountryRepository countryRepository)
    {
        _cityRepository = cityRepository;
        _countryRepository = countryRepository;

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("De naam mag niet leeg zijn")
            .MaximumLength(100)
            .WithMessage("De naam mag niet langer zijn dan 100 karakters");

        RuleFor(x => x.Population)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Het aantal inwoners mag niet negatief zijn")
            .LessThanOrEqualTo(50000000)
            .WithMessage("Het aantal inwoners mag niet groter zijn dan 50.000.000");

        RuleFor(x => x.CountryId)
            .GreaterThan(0)
            .WithMessage("Er moet een land gekozen worden")
            .MustAsync(async (countryId, cancellation) => await _countryRepository.GetCountryByIdAsync(countryId) != null)
            .WithMessage("Het gekozen land bestaat niet");
    }
}
