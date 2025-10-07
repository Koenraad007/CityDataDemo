using AP.CityDataDemo.Application.DTOs;

public interface ICountryService
{
    public Task<IEnumerable<CountryDto>> GetCountriesAsync();
    public Task<CountryDto?> GetCountryByIdAsync(int countryId, bool includeCities);
    public Task<CountryDto?> GetCountryByNameAsync(string countryName, bool includeCities);
    public Task<CountryDto> CreateCountryAsync(CountryDto country);
    public Task<bool> UpdateCountryAsync(int countryId, CountryDto country);
    public Task<bool> DeleteCountryAsync(int countryId);
}