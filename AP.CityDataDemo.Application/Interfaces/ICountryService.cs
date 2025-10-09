using AP.CityDataDemo.Shared.DTO;

namespace AP.CityDataDemo.Application.Interfaces
{
    public interface ICountryService
    {
        public Task<IEnumerable<CountryDto>> GetCountriesAsync();
        public Task<CountryDto?> GetCountryByIdAsync(int countryId, bool includeCities);
        public Task<CountryDto?> GetCountryByNameAsync(string countryName, bool includeCities);
        public Task<CountryDto> CreateCountryAsync(CountryDto countryDto);
        public Task<bool> UpdateCountryAsync(int countryId, CountryDto countryDto);
        public Task<bool> DeleteCountryAsync(int countryId);
    }
}