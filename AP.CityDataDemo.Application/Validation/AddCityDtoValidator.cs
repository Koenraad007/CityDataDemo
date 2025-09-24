using FluentValidation;
using AP.CityDataDemo.Application.DTOs;
using AP.CityDataDemo.Application.Interfaces;

namespace AP.CityDataDemo.Application.Validation;

public class AddCityDtoValidator : AbstractValidator<AddCityDto>
{
    private readonly ICityRepository _cityRepository;
    private readonly ICountryRepository _countryRepository;

    public AddCityDtoValidator(ICityRepository cityRepository, ICountryRepository countryRepository)
    {
        _cityRepository = cityRepository;
        _countryRepository = countryRepository;

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name cannot be empty")
            .MaximumLength(100)
            .WithMessage("Name cannot be longer than 100 characters")
            .MustAsync(async (name, cancellation) => !await _cityRepository.CityNameExistsAsync(name))
            .WithMessage("Name already exists");

        RuleFor(x => x.Population)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Population cannot be negative")
            .LessThanOrEqualTo(10000000000)
            .WithMessage("Population cannot be greater than 10,000,000,000");

        RuleFor(x => x.CountryId)
            .GreaterThan(0)
            .WithMessage("A country must be selected")
            .MustAsync(async (countryId, cancellation) => await _countryRepository.GetCountryByIdAsync(countryId) != null)
            .WithMessage("The selected country does not exist");
    }
}
