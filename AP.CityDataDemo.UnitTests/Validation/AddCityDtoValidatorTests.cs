using AP.CityDataDemo.Application.DTOs;
using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Application.Validation;
using AP.CityDataDemo.Domain.Entities;
using FluentValidation.TestHelper;
using Moq;

namespace AP.CityDataDemo.UnitTests.Validation;

[TestClass]
public class AddCityDtoValidatorTests
{
    private Mock<ICityRepository> _mockCityRepository = null!;
    private Mock<ICountryRepository> _mockCountryRepository = null!;
    private AddCityDtoValidator _validator = null!;

    [TestInitialize]
    public void Setup()
    {
        _mockCityRepository = new Mock<ICityRepository>();
        _mockCountryRepository = new Mock<ICountryRepository>();
        _validator = new AddCityDtoValidator(_mockCityRepository.Object, _mockCountryRepository.Object);
    }

    [TestMethod]
    public async Task Should_Have_Error_When_Name_Is_Empty()
    {
        _mockCityRepository.Setup(x => x.CityNameExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(false);
        _mockCountryRepository.Setup(x => x.GetCountryByIdAsync(1))
            .ReturnsAsync(new Country { Id = 1, Name = "Test Land" });

        var dto = new AddCityDto { Name = "", Population = 1000, CountryId = 1 };
        var result = await _validator.TestValidateAsync(dto);
        
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("De naam mag niet leeg zijn");
    }

    [TestMethod]
    public async Task Should_Have_Error_When_Name_Is_Too_Long()
    {
        _mockCityRepository.Setup(x => x.CityNameExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(false);
        _mockCountryRepository.Setup(x => x.GetCountryByIdAsync(1))
            .ReturnsAsync(new Country { Id = 1, Name = "Test Land" });

        var longName = new string('a', 101);
        var dto = new AddCityDto { Name = longName, Population = 1000, CountryId = 1 };
        var result = await _validator.TestValidateAsync(dto);
        
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("De naam mag niet langer zijn dan 100 karakters");
    }

    [TestMethod]
    public async Task Should_Have_Error_When_Name_Already_Exists()
    {
        _mockCityRepository.Setup(x => x.CityNameExistsAsync("Bestaande Stad"))
            .ReturnsAsync(true);
        _mockCountryRepository.Setup(x => x.GetCountryByIdAsync(1))
            .ReturnsAsync(new Country { Id = 1, Name = "Test Land" });

        var dto = new AddCityDto { Name = "Bestaande Stad", Population = 1000, CountryId = 1 };
        var result = await _validator.TestValidateAsync(dto);
        
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("De naam mag nog niet bestaan");
    }

    [TestMethod]
    public async Task Should_Have_Error_When_Population_Is_Negative()
    {
        _mockCityRepository.Setup(x => x.CityNameExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(false);
        _mockCountryRepository.Setup(x => x.GetCountryByIdAsync(1))
            .ReturnsAsync(new Country { Id = 1, Name = "Test Land" });

        var dto = new AddCityDto { Name = "Test Stad", Population = -1, CountryId = 1 };
        var result = await _validator.TestValidateAsync(dto);
        
        result.ShouldHaveValidationErrorFor(x => x.Population)
            .WithErrorMessage("Het aantal inwoners mag niet negatief zijn");
    }

    [TestMethod]
    public async Task Should_Have_Error_When_Population_Is_Too_High()
    {
        _mockCityRepository.Setup(x => x.CityNameExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(false);
        _mockCountryRepository.Setup(x => x.GetCountryByIdAsync(1))
            .ReturnsAsync(new Country { Id = 1, Name = "Test Land" });

        var dto = new AddCityDto { Name = "Test Stad", Population = 50000001, CountryId = 1 };
        var result = await _validator.TestValidateAsync(dto);
        
        result.ShouldHaveValidationErrorFor(x => x.Population)
            .WithErrorMessage("Het aantal inwoners mag niet groter zijn dan 50.000.000");
    }

    [TestMethod]
    public async Task Should_Have_Error_When_CountryId_Is_Zero()
    {
        _mockCityRepository.Setup(x => x.CityNameExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        var dto = new AddCityDto { Name = "Test Stad", Population = 1000, CountryId = 0 };
        var result = await _validator.TestValidateAsync(dto);
        
        result.ShouldHaveValidationErrorFor(x => x.CountryId)
            .WithErrorMessage("Er moet een land gekozen worden");
    }

    [TestMethod]
    public async Task Should_Have_Error_When_Country_Does_Not_Exist()
    {
        _mockCityRepository.Setup(x => x.CityNameExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(false);
        _mockCountryRepository.Setup(x => x.GetCountryByIdAsync(999))
            .ReturnsAsync((Country?)null);

        var dto = new AddCityDto { Name = "Test Stad", Population = 1000, CountryId = 999 };
        var result = await _validator.TestValidateAsync(dto);
        
        result.ShouldHaveValidationErrorFor(x => x.CountryId)
            .WithErrorMessage("Het gekozen land bestaat niet");
    }

    [TestMethod]
    public async Task Should_Not_Have_Error_When_All_Properties_Are_Valid()
    {
        _mockCityRepository.Setup(x => x.CityNameExistsAsync("Nieuwe Stad"))
            .ReturnsAsync(false);
        _mockCountryRepository.Setup(x => x.GetCountryByIdAsync(1))
            .ReturnsAsync(new Country { Id = 1, Name = "Test Land" });

        var dto = new AddCityDto { Name = "Nieuwe Stad", Population = 1000, CountryId = 1 };
        var result = await _validator.TestValidateAsync(dto);
        
        result.ShouldNotHaveAnyValidationErrors();
    }
}
