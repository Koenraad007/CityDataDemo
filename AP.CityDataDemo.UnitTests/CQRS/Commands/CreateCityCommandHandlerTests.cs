using AP.CityDataDemo.Application.CQRS.Commands.Cities;
using AP.CityDataDemo.Application.DTOs;
using AP.CityDataDemo.Application.Exceptions;
using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Application.Validation;
using AP.CityDataDemo.Domain.Entities;
using FluentValidation;
using Moq;

namespace AP.CityDataDemo.UnitTests.CQRS.Commands;

[TestClass]
public class CreateCityCommandHandlerTests
{
    private Mock<ICityRepository> _mockCityRepository = null!;
    private Mock<ICountryRepository> _mockCountryRepository = null!;
    private Mock<IUnitOfWork> _mockUnitOfWork = null!;
    private AddCityDtoValidator _validator = null!;
    private CreateCityCommandHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        _mockCityRepository = new Mock<ICityRepository>();
        _mockCountryRepository = new Mock<ICountryRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _validator = new AddCityDtoValidator(_mockCityRepository.Object, _mockCountryRepository.Object);
        
        _handler = new CreateCityCommandHandler(
            _mockCityRepository.Object,
            _mockCountryRepository.Object,
            _mockUnitOfWork.Object,
            _validator);
    }

    [TestMethod]
    public async Task Handle_Should_Create_City_When_Valid()
    {
        var validDto = new AddCityDto { Name = "Test City", Population = 1000, CountryId = 1 };
        var country = new Country { Id = 1, Name = "Test Country" };
        var command = new CreateCityCommand(validDto);

        _mockCountryRepository.Setup(x => x.GetCountryByIdAsync(1))
            .ReturnsAsync(country);
        _mockCityRepository.Setup(x => x.CityNameExistsAsync("Test City"))
            .ReturnsAsync(false);
        _mockCityRepository.Setup(x => x.AddCityAsync(It.IsAny<City>()))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.AreEqual(validDto.Name, result.Name);
        Assert.AreEqual(validDto.Population, result.Population);
        Assert.AreEqual(validDto.CountryId, result.CountryId);
        Assert.AreEqual(country.Name, result.CountryName);

        _mockCityRepository.Verify(x => x.AddCityAsync(It.IsAny<City>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [TestMethod]
    public async Task Handle_Should_Throw_ValidationException_When_Name_Is_Empty()
    {
        _mockCountryRepository.Setup(x => x.GetCountryByIdAsync(1))
            .ReturnsAsync(new Country { Id = 1, Name = "Test Country" });

        var invalidDto = new AddCityDto { Name = "", Population = 1000, CountryId = 1 };
        var command = new CreateCityCommand(invalidDto);

        var exception = await Assert.ThrowsExceptionAsync<ValidationException>(
            () => _handler.Handle(command, CancellationToken.None));

        Assert.IsTrue(exception.Message.Contains("Name cannot be empty"));
        _mockCityRepository.Verify(x => x.AddCityAsync(It.IsAny<City>()), Times.Never);
    }

    [TestMethod]
    public async Task Handle_Should_Throw_ValidationException_When_Name_Already_Exists()
    {
        _mockCityRepository.Setup(x => x.CityNameExistsAsync("Existing City"))
            .ReturnsAsync(true);
        _mockCountryRepository.Setup(x => x.GetCountryByIdAsync(1))
            .ReturnsAsync(new Country { Id = 1, Name = "Test Country" });

        var invalidDto = new AddCityDto { Name = "Existing City", Population = 1000, CountryId = 1 };
        var command = new CreateCityCommand(invalidDto);

        var exception = await Assert.ThrowsExceptionAsync<ValidationException>(
            () => _handler.Handle(command, CancellationToken.None));

        Assert.IsTrue(exception.Message.Contains("Name already exists"));
        _mockCityRepository.Verify(x => x.AddCityAsync(It.IsAny<City>()), Times.Never);
    }

    [TestMethod]
    public async Task Handle_Should_Throw_ValidationException_When_Country_Does_Not_Exist()
    {
        _mockCityRepository.Setup(x => x.CityNameExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(false);
        _mockCountryRepository.Setup(x => x.GetCountryByIdAsync(999))
            .ReturnsAsync((Country?)null);

        var invalidDto = new AddCityDto { Name = "Test City", Population = 1000, CountryId = 999 };
        var command = new CreateCityCommand(invalidDto);

        var exception = await Assert.ThrowsExceptionAsync<ValidationException>(
            () => _handler.Handle(command, CancellationToken.None));

        Assert.IsTrue(exception.Message.Contains("The selected country does not exist"));
        _mockCityRepository.Verify(x => x.AddCityAsync(It.IsAny<City>()), Times.Never);
    }
}
