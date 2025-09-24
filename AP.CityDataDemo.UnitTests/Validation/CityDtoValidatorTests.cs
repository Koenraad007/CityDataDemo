using AP.CityDataDemo.Application.DTOs;
using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Application.Validation;
using AP.CityDataDemo.Domain.Entities;
using FluentValidation.TestHelper;
using Moq;

namespace AP.CityDataDemo.UnitTests.Validation;

[TestClass]
public class CityDtoValidatorTests
{
    private Mock<ICityRepository> _mockCityRepository = null!;
    private Mock<ICountryRepository> _mockCountryRepository = null!;
    private CityDtoValidator _validator = null!;

    [TestInitialize]
    public void Setup()
    {
        _mockCityRepository = new Mock<ICityRepository>();
        _mockCountryRepository = new Mock<ICountryRepository>();
        _validator = new CityDtoValidator(_mockCityRepository.Object, _mockCountryRepository.Object);
    }

    [TestMethod]
    public async Task Should_Have_Error_When_Name_Is_Empty()
    {
        _mockCountryRepository.Setup(x => x.GetCountryByIdAsync(1))
            .ReturnsAsync(new Country { Id = 1, Name = "Test Land" });

        var dto = new CityDto { Id = 1, Name = "", Population = 1000, CountryId = 1 };
        var result = await _validator.TestValidateAsync(dto);
        
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("De naam mag niet leeg zijn");
    }

    [TestMethod]
    public async Task Should_Have_Error_When_Name_Is_Too_Long()
    {
        _mockCountryRepository.Setup(x => x.GetCountryByIdAsync(1))
            .ReturnsAsync(new Country { Id = 1, Name = "Test Land" });

        var longName = new string('a', 101);
        var dto = new CityDto { Id = 1, Name = longName, Population = 1000, CountryId = 1 };
        var result = await _validator.TestValidateAsync(dto);
        
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("De naam mag niet langer zijn dan 100 karakters");
    }

    [TestMethod]
    public async Task Should_Have_Error_When_Population_Is_Negative()
    {
        _mockCountryRepository.Setup(x => x.GetCountryByIdAsync(1))
            .ReturnsAsync(new Country { Id = 1, Name = "Test Land" });

        var dto = new CityDto { Id = 1, Name = "Test Stad", Population = -1, CountryId = 1 };
        var result = await _validator.TestValidateAsync(dto);
        
        result.ShouldHaveValidationErrorFor(x => x.Population)
            .WithErrorMessage("Het aantal inwoners mag niet negatief zijn");
    }

    [TestMethod]
    public async Task Should_Have_Error_When_Population_Is_Too_High()
    {
        _mockCountryRepository.Setup(x => x.GetCountryByIdAsync(1))
            .ReturnsAsync(new Country { Id = 1, Name = "Test Land" });

        var dto = new CityDto { Id = 1, Name = "Test Stad", Population = 50000001, CountryId = 1 };
        var result = await _validator.TestValidateAsync(dto);
        
        result.ShouldHaveValidationErrorFor(x => x.Population)
            .WithErrorMessage("Het aantal inwoners mag niet groter zijn dan 50.000.000");
    }

    [TestMethod]
    public async Task Should_Have_Error_When_CountryId_Is_Zero()
    {
        var dto = new CityDto { Id = 1, Name = "Test Stad", Population = 1000, CountryId = 0 };
        var result = await _validator.TestValidateAsync(dto);
        
        result.ShouldHaveValidationErrorFor(x => x.CountryId)
            .WithErrorMessage("Er moet een land gekozen worden");
    }

    [TestMethod]
    public async Task Should_Have_Error_When_Country_Does_Not_Exist()
    {
        _mockCountryRepository.Setup(x => x.GetCountryByIdAsync(999))
            .ReturnsAsync((Country?)null);

        var dto = new CityDto { Id = 1, Name = "Test Stad", Population = 1000, CountryId = 999 };
        var result = await _validator.TestValidateAsync(dto);
        
        result.ShouldHaveValidationErrorFor(x => x.CountryId)
            .WithErrorMessage("Het gekozen land bestaat niet");
    }

    [TestMethod]
    public async Task Should_Not_Have_Error_When_All_Properties_Are_Valid()
    {
        _mockCountryRepository.Setup(x => x.GetCountryByIdAsync(1))
            .ReturnsAsync(new Country { Id = 1, Name = "Test Land" });

        var dto = new CityDto { Id = 1, Name = "Bestaande Stad", Population = 1000, CountryId = 1 };
        var result = await _validator.TestValidateAsync(dto);
        
        result.ShouldNotHaveAnyValidationErrors();
    }
}
