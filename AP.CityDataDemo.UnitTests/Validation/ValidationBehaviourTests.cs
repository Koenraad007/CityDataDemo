
using FluentValidation;
using AP.CityDataDemo.Application.Behaviours;
using MediatR;
using Moq;

namespace AP.CityDataDemo.UnitTests.Validation
{

    public interface ITestRequest : IRequest<string>
    {
        string? Value { get; set; }
    }

    public class TestRequestValidator : AbstractValidator<ITestRequest>
    {
        public TestRequestValidator()
        {
            RuleFor(x => x.Value).NotEmpty();
        }
    }

    [TestClass]
    public class ValidationBehaviourTests
    {

        [TestMethod]
        public async Task Handle_ValidRequest_CallsNext()
        {
            // Arrange
            var validators = new List<IValidator<ITestRequest>> { new TestRequestValidator() };
            var behaviour = new ValidationBehaviour<ITestRequest, string>(validators);
            var mockRequest = new Mock<ITestRequest>();
            mockRequest.SetupProperty(r => r.Value, "valid");
            var nextCalled = false;
            RequestHandlerDelegate<string> next = (ct) => { nextCalled = true; return Task.FromResult("ok"); };

            // Act
            var result = await behaviour.Handle(mockRequest.Object, next, CancellationToken.None);

            // Assert
            Assert.IsTrue(nextCalled);
            Assert.AreEqual("ok", result);
        }


        [TestMethod]
        public async Task Handle_InvalidRequest_ThrowsValidationException()
        {
            // Arrange
            var validators = new List<IValidator<ITestRequest>> { new TestRequestValidator() };
            var behaviour = new ValidationBehaviour<ITestRequest, string>(validators);
            var mockRequest = new Mock<ITestRequest>();
            mockRequest.SetupProperty(r => r.Value, "");
            RequestHandlerDelegate<string> next = (ct) => Task.FromResult("should not be called");

            // Act & Assert
            await Assert.ThrowsExceptionAsync<ValidationException>(async () =>
            {
                await behaviour.Handle(mockRequest.Object, next, CancellationToken.None);
            });
        }


        [TestMethod]
        public async Task Handle_NoValidators_CallsNext()
        {
            // Arrange
            var validators = new List<IValidator<ITestRequest>>();
            var behaviour = new ValidationBehaviour<ITestRequest, string>(validators);
            var mockRequest = new Mock<ITestRequest>();
            mockRequest.SetupProperty(r => r.Value, null);
            var nextCalled = false;
            RequestHandlerDelegate<string> next = (ct) => { nextCalled = true; return Task.FromResult("ok"); };

            // Act
            var result = await behaviour.Handle(mockRequest.Object, next, CancellationToken.None);

            // Assert
            Assert.IsTrue(nextCalled);
            Assert.AreEqual("ok", result);
        }
    }
}