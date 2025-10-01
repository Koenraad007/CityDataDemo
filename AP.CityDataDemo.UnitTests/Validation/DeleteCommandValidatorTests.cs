using Moq;
using AP.CityDataDemo.Application.CQRS.Commands.Cities;
using AP.CityDataDemo.Application.Interfaces;

namespace AP.CityDataDemo.UnitTests.Validation
{
    [TestClass]
    public class DeleteCommandValidatorTests
    {
        private Mock<IUnitOfWork> _mockUow;
        private Mock<ICityRepository> _mockRepo;

        [TestInitialize]
        public void Setup()
        {
            _mockUow = new Mock<IUnitOfWork>();
            _mockRepo = new Mock<ICityRepository>();
            _mockUow.Setup(u => u.CitiesRepository).Returns(_mockRepo.Object);
        }

        [TestMethod]
        public async Task Validator_Fails_WhenDeletingLastCity()
        {
            _mockRepo.Setup(r => r.GetCountAsync(CancellationToken.None)).ReturnsAsync(1);
            var validator = new DeleteCommandValidator(_mockUow.Object);
            var command = new DeleteCommand(1);

            var result = await validator.ValidateAsync(command);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Exists(e => e.ErrorMessage.Contains("Cannot delete the last city")));
        }

        [TestMethod]
        public async Task Validator_Succeeds_WhenMultipleCitiesExist()
        {
            _mockRepo.Setup(r => r.GetCountAsync(CancellationToken.None)).ReturnsAsync(2);
            var validator = new DeleteCommandValidator(_mockUow.Object);
            var command = new DeleteCommand(1);

            var result = await validator.ValidateAsync(command);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public async Task Validator_Succeeds_WhenManyCitiesExist()
        {
            _mockRepo.Setup(r => r.GetCountAsync(CancellationToken.None)).ReturnsAsync(100);
            var validator = new DeleteCommandValidator(_mockUow.Object);
            var command = new DeleteCommand(1);

            var result = await validator.ValidateAsync(command);

            Assert.IsTrue(result.IsValid);
        }
    }
}

