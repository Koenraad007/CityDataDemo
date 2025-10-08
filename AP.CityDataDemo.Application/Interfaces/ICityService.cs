using AP.CityDataDemo.Shared.DTO;

public interface ICityService
{
    public Task<IEnumerable<CityDto>> GetCitiesAsync();
    public Task<CityDto?> GetCityByIdAsync(int cityId, bool includePointsOfInterest);
    public Task<CityDto?> GetCityByNameAsync(string cityName, bool includePointsOfInterest);
    public Task<CityDto> CreateCityAsync(CityDto city);
    public Task<bool> UpdateCityAsync(int cityId, CityDto city);
    public Task<bool> DeleteCityAsync(int cityId);
}