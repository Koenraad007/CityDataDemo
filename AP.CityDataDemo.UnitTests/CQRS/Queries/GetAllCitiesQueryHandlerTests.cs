using AP.CityDataDemo.Application.CQRS.Queries.Cities;
using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Domain;
using Moq;

namespace AP.CityDataDemo.UnitTests.CQRS.Queries;

[TestClass]
public class GetAllCitiesQueryHandlerTests
{
    private Mock<ICityRepository> _mockCityRepository = null!;
    private Mock<ICountryRepository> _mockCountryRepository = null!;
    private GetAllCitiesQueryHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        _mockCityRepository = new Mock<ICityRepository>();
        _mockCountryRepository = new Mock<ICountryRepository>();

        _handler = new GetAllCitiesQueryHandler(
            _mockCityRepository.Object,
            _mockCountryRepository.Object);
    }

    [TestMethod]
    public async Task Handle_Should_Return_All_Cities_With_Country_Names()
    {
        var cities = new List<City>
        {
            new("New York", 8000000, 1) { Id = 1 },
            new("Los Angeles", 4000000, 1) { Id = 2 }
        };
        var countries = new List<Country>
        {
            new() { Id = 1, Name = "USA" }
        };

        _mockCityRepository.Setup(x => x.GetAllAsync(true, false))
            .ReturnsAsync(cities);
        _mockCountryRepository.Setup(x => x.GetAllCountriesAsync())
            .ReturnsAsync(countries);

        var query = new GetAllCitiesQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.AreEqual(2, result.Count());
        Assert.IsTrue(result.All(c => c.CountryName == "USA"));

        _mockCityRepository.Verify(x => x.GetAllAsync(true, false), Times.Once);
        _mockCountryRepository.Verify(x => x.GetAllCountriesAsync(), Times.Once);
    }
}
