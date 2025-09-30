using MediatR;
using AP.CityDataDemo.Application.DTOs;
using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Application.Mappings;
using FluentValidation;
using AP.CityDataDemo.Domain;

namespace AP.CityDataDemo.Application.CQRS.Commands.Cities;

public class CreateCityCommandHandler : IRequestHandler<CreateCityCommand, CityDto>
{
    private readonly ICityRepository _cityRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<AddCityDto> _validator;

    public CreateCityCommandHandler(
        ICityRepository cityRepository,
        ICountryRepository countryRepository,
        IUnitOfWork unitOfWork,
        IValidator<AddCityDto> validator)
    {
        _cityRepository = cityRepository;
        _countryRepository = countryRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<CityDto> Handle(CreateCityCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request.AddCityDto, cancellationToken);

        var city = new City() { Name = request.AddCityDto.Name, Population = (int)request.AddCityDto.Population, CountryId = request.AddCityDto.CountryId };
        await _cityRepository.AddCityAsync(city);
        await _unitOfWork.Commit();

        var country = await _countryRepository.GetCountryByIdAsync(request.AddCityDto.CountryId);
        var resultDto = city!.ToDto();
        resultDto.CountryName = country?.Name ?? "N/A";
        return resultDto;
    }
}
