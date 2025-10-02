using AP.CityDataDemo.Application.DTOs;
using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Application.Validation;
using AP.CityDataDemo.Domain;
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
        var dto = new AddCityDto { Name = "", Population = 1000, CountryId = 1 };
        var result = await _validator.TestValidateAsync(dto);

        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("Name cannot be empty");
    }

    [TestMethod]
    public async Task Should_Have_Error_When_Name_Exceeds_MaxLength()
    {
        _mockCityRepository.Setup(x => x.CityNameExistsAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(false);

        var dto = new AddCityDto { Name = new string('a', 101), Population = 1000, CountryId = 1 };
        var result = await _validator.TestValidateAsync(dto);

        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("Name cannot be longer than 100 characters");
    }

    [TestMethod]
    public async Task Should_Have_Error_When_Name_Already_Exists()
    {
        _mockCityRepository.Setup(x => x.CityNameExistsAsync("Existing City", CancellationToken.None))
            .ReturnsAsync(true);

        var dto = new AddCityDto { Name = "Existing City", Population = 1000, CountryId = 1 };
        var result = await _validator.TestValidateAsync(dto);

        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("Name already exists");
    }

    [TestMethod]
    public async Task Should_Have_Error_When_Population_Is_Negative()
    {
        _mockCityRepository.Setup(x => x.CityNameExistsAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(false);

        var dto = new AddCityDto { Name = "Test City", Population = -1, CountryId = 1 };
        var result = await _validator.TestValidateAsync(dto);

        result.ShouldHaveValidationErrorFor(x => x.Population)
            .WithErrorMessage("Population cannot be negative");
    }

    [TestMethod]
    public async Task Should_Have_Error_When_Population_Is_Too_High()
    {
        _mockCityRepository.Setup(x => x.CityNameExistsAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(false);
        _mockCountryRepository.Setup(x => x.GetCountryByIdAsync(1, CancellationToken.None))
            .ReturnsAsync(new Country { Id = 1, Name = "Test Country" });

        var dto = new AddCityDto { Name = "Test City", Population = 10000000001, CountryId = 1 };
        var result = await _validator.TestValidateAsync(dto);

        result.ShouldHaveValidationErrorFor(x => x.Population)
            .WithErrorMessage("Population cannot be greater than 10,000,000,000");
    }

    [TestMethod]
    public async Task Should_Have_Error_When_CountryId_Is_Not_Positive()
    {
        _mockCityRepository.Setup(x => x.CityNameExistsAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(false);

        var dto = new AddCityDto { Name = "Test City", Population = 1000, CountryId = 0 };
        var result = await _validator.TestValidateAsync(dto);

        result.ShouldHaveValidationErrorFor(x => x.CountryId)
            .WithErrorMessage("A country must be selected");
    }

    [TestMethod]
    public async Task Should_Have_Error_When_Country_Does_Not_Exist()
    {
        _mockCityRepository.Setup(x => x.CityNameExistsAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(false);
        _mockCountryRepository.Setup(x => x.GetCountryByIdAsync(999, CancellationToken.None))
            .ReturnsAsync((Country?)null);

        var dto = new AddCityDto { Name = "Test City", Population = 1000, CountryId = 999 };
        var result = await _validator.TestValidateAsync(dto);

        result.ShouldHaveValidationErrorFor(x => x.CountryId)
            .WithErrorMessage("The selected country does not exist");
    }

    [TestMethod]
    public async Task Should_Not_Have_Error_When_All_Valid()
    {
        _mockCityRepository.Setup(x => x.CityNameExistsAsync("Valid City", CancellationToken.None))
            .ReturnsAsync(false);
        _mockCountryRepository.Setup(x => x.GetCountryByIdAsync(1, CancellationToken.None))
            .ReturnsAsync(new Country { Id = 1, Name = "Test Country" });

        var dto = new AddCityDto { Name = "Valid City", Population = 1000, CountryId = 1 };
        var result = await _validator.TestValidateAsync(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
