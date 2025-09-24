using AP.CityDataDemo.Application.DTOs;
using AP.CityDataDemo.Application.Exceptions;
using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Application.Services;
using AP.CityDataDemo.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace AP.CityDataDemo.UnitTests.Services;

[TestClass]
public class CityServiceTests
{
    private Mock<ICityRepository> _mockCityRepository = null!;
    private Mock<ICountryRepository> _mockCountryRepository = null!;
    private Mock<IUnitOfWork> _mockUnitOfWork = null!;
    private Mock<IValidator<AddCityDto>> _mockAddCityValidator = null!;
    private Mock<IValidator<CityDto>> _mockCityValidator = null!;
    private CityService _cityService = null!;

    [TestInitialize]
    public void Setup()
    {
        _mockCityRepository = new Mock<ICityRepository>();
        _mockCountryRepository = new Mock<ICountryRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockAddCityValidator = new Mock<IValidator<AddCityDto>>();
        _mockCityValidator = new Mock<IValidator<CityDto>>();

        _cityService = new CityService(
            _mockCityRepository.Object,
            _mockCountryRepository.Object,
            _mockUnitOfWork.Object,
            _mockAddCityValidator.Object,
            _mockCityValidator.Object);
    }

    [TestMethod]
    public async Task CreateCityAsync_Should_Throw_ArgumentException_When_AddCityDto_Has_Domain_Validation_Errors()
    {
        var invalidDto = new AddCityDto { Name = "Test", Population = -1, CountryId = 1 };
        var validationResult = new ValidationResult();
        
        _mockAddCityValidator.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<AddCityDto>>(), default))
            .ReturnsAsync(validationResult);

        await Assert.ThrowsExceptionAsync<ArgumentException>(
            () => _cityService.CreateCityAsync(invalidDto));

        _mockCityRepository.Verify(x => x.AddCityAsync(It.IsAny<City>()), Times.Never);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [TestMethod]
    public async Task CreateCityAsync_Should_Create_City_When_Valid()
    {
        var validDto = new AddCityDto { Name = "Test Stad", Population = 1000, CountryId = 1 };
        var country = new Country { Id = 1, Name = "Test Land" };
        var validationResult = new ValidationResult();

        _mockAddCityValidator.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<AddCityDto>>(), default))
            .ReturnsAsync(validationResult);

        _mockCountryRepository.Setup(x => x.GetCountryByIdAsync(1))
            .ReturnsAsync(country);

        _mockCityRepository.Setup(x => x.AddCityAsync(It.IsAny<City>()))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        var result = await _cityService.CreateCityAsync(validDto);

        Assert.AreEqual(validDto.Name, result.Name);
        Assert.AreEqual(validDto.Population, result.Population);
        Assert.AreEqual(validDto.CountryId, result.CountryId);
        Assert.AreEqual(country.Name, result.CountryName);

        _mockAddCityValidator.Verify(x => x.ValidateAsync(It.IsAny<ValidationContext<AddCityDto>>(), default), Times.Once);
        _mockCityRepository.Verify(x => x.AddCityAsync(It.IsAny<City>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [TestMethod]
    public async Task UpdateCityAsync_Should_Throw_ArgumentException_When_CityDto_Has_Domain_Validation_Errors()
    {
        var invalidDto = new CityDto { Id = 1, Name = "Test", Population = -1, CountryId = 1 };
        var validationResult = new ValidationResult();
        
        _mockCityValidator.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<CityDto>>(), default))
            .ReturnsAsync(validationResult);

        await Assert.ThrowsExceptionAsync<ArgumentException>(
            () => _cityService.UpdateCityAsync(1, invalidDto));

        _mockCityRepository.Verify(x => x.UpdateCityAsync(It.IsAny<City>()), Times.Never);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [TestMethod]
    public async Task UpdateCityAsync_Should_Throw_NotFoundException_When_City_Not_Found()
    {
        var validDto = new CityDto { Id = 1, Name = "Test Stad", Population = 1000, CountryId = 1 };
        var validationResult = new ValidationResult();

        _mockCityValidator.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<CityDto>>(), default))
            .ReturnsAsync(validationResult);

        _mockCityRepository.Setup(x => x.UpdateCityAsync(It.IsAny<City>()))
            .ReturnsAsync(false);

        var exception = await Assert.ThrowsExceptionAsync<NotFoundException>(
            () => _cityService.UpdateCityAsync(1, validDto));

        Assert.AreEqual("Stad met id 1 werd niet gevonden", exception.Message);
        
        _mockCityValidator.Verify(x => x.ValidateAsync(It.IsAny<ValidationContext<CityDto>>(), default), Times.Once);
        _mockCityRepository.Verify(x => x.UpdateCityAsync(It.IsAny<City>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [TestMethod]
    public async Task UpdateCityAsync_Should_Update_City_When_Valid_And_Exists()
    {
        var validDto = new CityDto { Id = 1, Name = "Test Stad", Population = 1000, CountryId = 1 };
        var country = new Country { Id = 1, Name = "Test Land" };
        var validationResult = new ValidationResult();

        _mockCityValidator.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<CityDto>>(), default))
            .ReturnsAsync(validationResult);

        _mockCityRepository.Setup(x => x.UpdateCityAsync(It.IsAny<City>()))
            .ReturnsAsync(true);

        _mockCountryRepository.Setup(x => x.GetCountryByIdAsync(1))
            .ReturnsAsync(country);

        _mockUnitOfWork.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        var result = await _cityService.UpdateCityAsync(1, validDto);

        Assert.AreEqual(validDto.Name, result.Name);
        Assert.AreEqual(validDto.Population, result.Population);
        Assert.AreEqual(validDto.CountryId, result.CountryId);
        Assert.AreEqual(country.Name, result.CountryName);

        _mockCityValidator.Verify(x => x.ValidateAsync(It.IsAny<ValidationContext<CityDto>>(), default), Times.Once);
        _mockCityRepository.Verify(x => x.UpdateCityAsync(It.IsAny<City>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [TestMethod]
    public async Task DeleteCityAsync_Should_Throw_NotFoundException_When_City_Not_Found()
    {
        _mockCityRepository.Setup(x => x.DeleteCityByIdAsync(1))
            .ReturnsAsync(false);

        var exception = await Assert.ThrowsExceptionAsync<NotFoundException>(
            () => _cityService.DeleteCityAsync(1));

        Assert.AreEqual("Stad met id 1 werd niet gevonden", exception.Message);
        
        _mockCityRepository.Verify(x => x.DeleteCityByIdAsync(1), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [TestMethod]
    public async Task DeleteCityAsync_Should_Delete_City_When_Exists()
    {
        _mockCityRepository.Setup(x => x.DeleteCityByIdAsync(1))
            .ReturnsAsync(true);

        _mockUnitOfWork.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        await _cityService.DeleteCityAsync(1);

        _mockCityRepository.Verify(x => x.DeleteCityByIdAsync(1), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
}
