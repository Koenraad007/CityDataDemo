using Moq;
using AP.CityDataDemo.Application.CQRS.Commands.Cities;
using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Domain;

namespace AP.CityDataDemo.UnitTests.Validation
{

    [TestClass]
    public class EditCityCommandTests
    {
        private Mock<IUnitOfWork> _mockUow;
        private Mock<ICitiesRepository> _mockRepo;

        [TestInitialize]
        public void Setup()
        {
            _mockUow = new Mock<IUnitOfWork>();
            _mockRepo = new Mock<ICitiesRepository>();
            _mockUow.Setup(u => u.CitiesRepository).Returns(_mockRepo.Object);
        }

        [TestMethod]
        public async Task Handler_UpdatesCity_WhenCityExists()
        {
            var city = new City { Id = 1, Name = "TestCity", Population = 500, CountryId = 1 };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(city);
            _mockUow.Setup(u => u.Commit()).Returns(Task.CompletedTask);
            var handler = new EditCityCommandHandler(_mockUow.Object);
            var command = new EditCityCommand(1, "TestCity", 1000, 2);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("TestCity", result.Name);
            Assert.AreEqual(1000, result.Population);
            Assert.AreEqual(2, result.CountryId);
            _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<City>()), Times.Once);
            _mockUow.Verify(u => u.Commit(), Times.Once);
        }

        [TestMethod]
        public async Task Validator_Fails_WhenNameIsEmpty()
        {
            var validator = new EditCityCommandValidator(_mockUow.Object);
            var command = new EditCityCommand(1, "", 100, 1);

            var result = await validator.ValidateAsync(command);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Exists(e => e.PropertyName == "Name"));
        }

        [TestMethod]
        public async Task Validator_Fails_WhenPopulationIsNegative()
        {
            var validator = new EditCityCommandValidator(_mockUow.Object);
            var command = new EditCityCommand(1, "TestCity", -1, 1);

            var result = await validator.ValidateAsync(command);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Exists(e => e.PropertyName == "Population"));
        }

        [TestMethod]
        public async Task Validator_Fails_WhenCountryIdIsNotPositive()
        {
            var validator = new EditCityCommandValidator(_mockUow.Object);
            var command = new EditCityCommand(1, "TestCity", 100, 0);

            var result = await validator.ValidateAsync(command);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Exists(e => e.PropertyName == "CountryId"));
        }

        [TestMethod]
        public async Task Validator_Fails_WhenNameIsChanged()
        {
            var city = new City { Id = 1, Name = "OriginalName", Population = 100, CountryId = 1 };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(city);
            var validator = new EditCityCommandValidator(_mockUow.Object);
            var command = new EditCityCommand(1, "ChangedName", 100, 1);

            var result = await validator.ValidateAsync(command);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Exists(e => e.ErrorMessage.Contains("cannot be changed")));
        }

        [TestMethod]
        public async Task Validator_Succeeds_WhenAllValid()
        {
            var city = new City { Id = 1, Name = "TestCity", Population = 100, CountryId = 1 };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(city);
            var validator = new EditCityCommandValidator(_mockUow.Object);
            var command = new EditCityCommand(1, "TestCity", 200, 2);

            var result = await validator.ValidateAsync(command);

            Assert.IsTrue(result.IsValid);
        }
    }
}
