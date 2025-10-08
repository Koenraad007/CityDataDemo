using Moq;
using AP.CityDataDemo.Application.CQRS.Commands.Cities;
using AP.CityDataDemo.Shared.DTO;
using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Domain;

namespace AP.CityDataDemo.UnitTests.Validation
{
    [TestClass]
    public class CreateCityCommandValidatorTests
    {
        private Mock<ICityRepository> _mockCityRepository = null!;
        private Mock<ICountryRepository> _mockCountryRepository = null!;
        private CreateCityCommandValidator _validator = null!;

        [TestInitialize]
        public void Setup()
        {
            _mockCityRepository = new Mock<ICityRepository>();
            _mockCountryRepository = new Mock<ICountryRepository>();
            _validator = new CreateCityCommandValidator(_mockCityRepository.Object, _mockCountryRepository.Object);
        }

        [TestMethod]
        public async Task Validator_Fails_WhenNameAlreadyExists()
        {
            _mockCityRepository.Setup(r => r.CityNameExistsAsync("ExistingCity", default)).ReturnsAsync(true);
            var dto = new AddCityDto { Name = "ExistingCity", Population = 100, CountryId = 1 };
            var command = new CreateCityCommand(dto);

            var result = await _validator.ValidateAsync(command);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Exists(e => e.ErrorMessage.Contains("already exists")));
        }

        [TestMethod]
        public async Task Validator_Succeeds_WhenAllValid()
        {
            _mockCityRepository.Setup(r => r.CityNameExistsAsync("ValidCity", default)).ReturnsAsync(false);
            _mockCountryRepository.Setup(r => r.GetCountryByIdAsync(1, default)).ReturnsAsync(new Country { Id = 1, Name = "Belgium" });
            var dto = new AddCityDto { Name = "ValidCity", Population = 100000, CountryId = 1 };
            var command = new CreateCityCommand(dto);

            var result = await _validator.ValidateAsync(command);

            Assert.IsTrue(result.IsValid);
        }
    }
}

