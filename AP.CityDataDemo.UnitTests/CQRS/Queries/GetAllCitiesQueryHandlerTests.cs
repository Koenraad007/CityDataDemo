using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Domain;
using AP.CityDataDemo.Application.CQRS.Queries.Cities;
using Moq;

namespace AP.CityDataDemo.UnitTests.CQRS.Queries;

[TestClass]
public class GetAllCitiesQueryHandlerTests
{
    private Mock<IUnitOfWork> _mockUnitOfWork = null!;
    private GetAllCitiesQueryHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _handler = new GetAllCitiesQueryHandler(
            _mockUnitOfWork.Object);
    }

    [TestMethod]
    public async Task Handle_Should_Return_All_Cities_With_Country_Names()
    {
        var cities = new List<City>
        {
            new() {Name ="New York", Population = 8000000, CountryId = 1, Id = 1 },
            new() {Name ="Los Angeles", Population = 4000000, CountryId = 1, Id = 2 }
        };
        var countries = new List<Country>
        {
            new() { Id = 1, Name = "USA" }
        };

        _mockUnitOfWork.Setup(x => x.CitiesRepository.GetAllAsync(true, false))
            .ReturnsAsync(cities);
        _mockUnitOfWork.Setup(x => x.CountriesRepository.GetAllAsync())
            .ReturnsAsync(countries);

        var query = new GetAllCitiesQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.AreEqual(2, result.Count());
        Assert.IsTrue(result.All(c => c.CountryName == "USA"));

        _mockUnitOfWork.Verify(x => x.CitiesRepository.GetAllAsync(true, false), Times.Once);
        _mockUnitOfWork.Verify(x => x.CountriesRepository.GetAllAsync(), Times.Once);
    }
}
