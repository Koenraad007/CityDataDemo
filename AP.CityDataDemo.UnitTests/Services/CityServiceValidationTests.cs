using AP.CityDataDemo.Application.DTOs;
using AP.CityDataDemo.Application.Exceptions;
using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Application.Services;
using AP.CityDataDemo.Application.Validation;
using AP.CityDataDemo.Domain.Entities;
using FluentValidation;
using Moq;

namespace AP.CityDataDemo.UnitTests.Services;

[TestClass]
public class CityServiceValidationTests
{
    private Mock<ICityRepository> _mockCityRepository = null!;
    private Mock<ICountryRepository> _mockCountryRepository = null!;
    private Mock<IUnitOfWork> _mockUnitOfWork = null!;
    private AddCityDtoValidator _addCityValidator = null!;
    private CityDtoValidator _cityValidator = null!;
    private CityService _cityService = null!;

    [TestInitialize]
    public void Setup()
    {
        _mockCityRepository = new Mock<ICityRepository>();
        _mockCountryRepository = new Mock<ICountryRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _addCityValidator = new AddCityDtoValidator(_mockCityRepository.Object, _mockCountryRepository.Object);
        _cityValidator = new CityDtoValidator(_mockCityRepository.Object, _mockCountryRepository.Object);

        _cityService = new CityService(
            _mockCityRepository.Object,
            _mockCountryRepository.Object,
            _mockUnitOfWork.Object,
            _addCityValidator,
            _cityValidator);
    }

    [TestMethod]
    public async Task CreateCityAsync_Should_Throw_ValidationException_When_Name_Is_Empty()
    {
        _mockCountryRepository.Setup(x => x.GetCountryByIdAsync(1))
            .ReturnsAsync(new Country { Id = 1, Name = "Test Land" });

        var invalidDto = new AddCityDto { Name = "", Population = 1000, CountryId = 1 };

        var exception = await Assert.ThrowsExceptionAsync<ValidationException>(
            () => _cityService.CreateCityAsync(invalidDto));

        Assert.IsTrue(exception.Message.Contains("De naam mag niet leeg zijn"));
        _mockCityRepository.Verify(x => x.AddCityAsync(It.IsAny<City>()), Times.Never);
    }

    [TestMethod]
    public async Task CreateCityAsync_Should_Throw_ValidationException_When_Name_Too_Long()
    {
        _mockCountryRepository.Setup(x => x.GetCountryByIdAsync(1))
            .ReturnsAsync(new Country { Id = 1, Name = "Test Land" });

        var longName = new string('a', 101);
        var invalidDto = new AddCityDto { Name = longName, Population = 1000, CountryId = 1 };

        var exception = await Assert.ThrowsExceptionAsync<ValidationException>(
            () => _cityService.CreateCityAsync(invalidDto));

        Assert.IsTrue(exception.Message.Contains("De naam mag niet langer zijn dan 100 karakters"));
        _mockCityRepository.Verify(x => x.AddCityAsync(It.IsAny<City>()), Times.Never);
    }

    [TestMethod]
    public async Task CreateCityAsync_Should_Throw_ValidationException_When_Name_Already_Exists()
    {
        _mockCityRepository.Setup(x => x.CityNameExistsAsync("Bestaande Stad"))
            .ReturnsAsync(true);
        _mockCountryRepository.Setup(x => x.GetCountryByIdAsync(1))
            .ReturnsAsync(new Country { Id = 1, Name = "Test Land" });

        var invalidDto = new AddCityDto { Name = "Bestaande Stad", Population = 1000, CountryId = 1 };

        var exception = await Assert.ThrowsExceptionAsync<ValidationException>(
            () => _cityService.CreateCityAsync(invalidDto));

        Assert.IsTrue(exception.Message.Contains("De naam mag nog niet bestaan"));
        _mockCityRepository.Verify(x => x.AddCityAsync(It.IsAny<City>()), Times.Never);
    }

    [TestMethod]
    public async Task CreateCityAsync_Should_Throw_ValidationException_When_Population_Too_High()
    {
        _mockCountryRepository.Setup(x => x.GetCountryByIdAsync(1))
            .ReturnsAsync(new Country { Id = 1, Name = "Test Land" });

        var invalidDto = new AddCityDto { Name = "Test Stad", Population = 50000001, CountryId = 1 };

        var exception = await Assert.ThrowsExceptionAsync<ValidationException>(
            () => _cityService.CreateCityAsync(invalidDto));

        Assert.IsTrue(exception.Message.Contains("Het aantal inwoners mag niet groter zijn dan 50.000.000"));
        _mockCityRepository.Verify(x => x.AddCityAsync(It.IsAny<City>()), Times.Never);
    }

    [TestMethod]
    public async Task CreateCityAsync_Should_Throw_ValidationException_When_CountryId_Is_Zero()
    {
        var invalidDto = new AddCityDto { Name = "Test Stad", Population = 1000, CountryId = 0 };

        var exception = await Assert.ThrowsExceptionAsync<ValidationException>(
            () => _cityService.CreateCityAsync(invalidDto));

        Assert.IsTrue(exception.Message.Contains("Er moet een land gekozen worden"));
        _mockCityRepository.Verify(x => x.AddCityAsync(It.IsAny<City>()), Times.Never);
    }

    [TestMethod]
    public async Task CreateCityAsync_Should_Throw_ValidationException_When_Country_Does_Not_Exist()
    {
        _mockCityRepository.Setup(x => x.CityNameExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(false);
        _mockCountryRepository.Setup(x => x.GetCountryByIdAsync(999))
            .ReturnsAsync((Country?)null);

        var invalidDto = new AddCityDto { Name = "Test Stad", Population = 1000, CountryId = 999 };

        var exception = await Assert.ThrowsExceptionAsync<ValidationException>(
            () => _cityService.CreateCityAsync(invalidDto));

        Assert.IsTrue(exception.Message.Contains("Het gekozen land bestaat niet"));
        _mockCityRepository.Verify(x => x.AddCityAsync(It.IsAny<City>()), Times.Never);
    }

    [TestMethod]
    public async Task UpdateCityAsync_Should_Throw_ValidationException_When_Name_Is_Empty()
    {
        _mockCountryRepository.Setup(x => x.GetCountryByIdAsync(1))
            .ReturnsAsync(new Country { Id = 1, Name = "Test Land" });

        var invalidDto = new CityDto { Id = 1, Name = "", Population = 1000, CountryId = 1 };

        var exception = await Assert.ThrowsExceptionAsync<ValidationException>(
            () => _cityService.UpdateCityAsync(1, invalidDto));

        Assert.IsTrue(exception.Message.Contains("De naam mag niet leeg zijn"));
        _mockCityRepository.Verify(x => x.UpdateCityAsync(It.IsAny<City>()), Times.Never);
    }

    [TestMethod]
    public async Task UpdateCityAsync_Should_Throw_ValidationException_When_Population_Too_High()
    {
        _mockCountryRepository.Setup(x => x.GetCountryByIdAsync(1))
            .ReturnsAsync(new Country { Id = 1, Name = "Test Land" });

        var invalidDto = new CityDto { Id = 1, Name = "Test Stad", Population = 50000001, CountryId = 1 };

        var exception = await Assert.ThrowsExceptionAsync<ValidationException>(
            () => _cityService.UpdateCityAsync(1, invalidDto));

        Assert.IsTrue(exception.Message.Contains("Het aantal inwoners mag niet groter zijn dan 50.000.000"));
        _mockCityRepository.Verify(x => x.UpdateCityAsync(It.IsAny<City>()), Times.Never);
    }

    [TestMethod]
    public async Task UpdateCityAsync_Should_Throw_ValidationException_When_Country_Does_Not_Exist()
    {
        _mockCountryRepository.Setup(x => x.GetCountryByIdAsync(999))
            .ReturnsAsync((Country?)null);

        var invalidDto = new CityDto { Id = 1, Name = "Test Stad", Population = 1000, CountryId = 999 };

        var exception = await Assert.ThrowsExceptionAsync<ValidationException>(
            () => _cityService.UpdateCityAsync(1, invalidDto));

        Assert.IsTrue(exception.Message.Contains("Het gekozen land bestaat niet"));
        _mockCityRepository.Verify(x => x.UpdateCityAsync(It.IsAny<City>()), Times.Never);
    }
}
