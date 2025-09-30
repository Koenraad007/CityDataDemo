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
    public async Task Should_Have_Error_When_Population_Is_Too_High()
    {
        _mockCityRepository.Setup(x => x.CityNameExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(false);
        _mockCountryRepository.Setup(x => x.GetCountryByIdAsync(1))
            .ReturnsAsync(new Country { Id = 1, Name = "Test Country" });

        var dto = new AddCityDto { Name = "Test City", Population = 10000000001, CountryId = 1 };
        var result = await _validator.TestValidateAsync(dto);

        result.ShouldHaveValidationErrorFor(x => x.Population)
            .WithErrorMessage("Population cannot be greater than 10,000,000,000");
    }
}
