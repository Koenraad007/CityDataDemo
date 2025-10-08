using AP.CityDataDemo.Shared.DTO;
using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Domain;
using AutoMapper;

namespace AP.CityDataDemo.Application.Services
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _cityRepository;
        private readonly IMapper _mapper;

        public CityService(ICityRepository cityRepository, IMapper mapper)
        {
            _cityRepository = cityRepository ?? throw new ArgumentNullException(nameof(cityRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<CityDto>> GetCitiesAsync()
        {
            var cities = await _cityRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CityDto>>(cities);
        }

        public async Task<CityDto?> GetCityByIdAsync(int cityId, bool includePointsOfInterest)
        {
            var city = await _cityRepository.GetCityByIdAsync(cityId);
            return city == null ? null : _mapper.Map<CityDto>(city);
        }

        public async Task<CityDto> CreateCityAsync(CityDto cityDto)
        {
            var cityEntity = _mapper.Map<City>(cityDto);
            await _cityRepository.AddCityAsync(cityEntity);
            return _mapper.Map<CityDto>(cityEntity);
        }

        public async Task<bool> UpdateCityAsync(int cityId, CityDto cityDto)
        {
            var existingCity = await _cityRepository.GetCityByIdAsync(cityId);
            if (existingCity == null)
            {
                return false;
            }
            _mapper.Map(cityDto, existingCity);
            await _cityRepository.UpdateAsync(existingCity);
            return true;
        }

        public async Task<bool> DeleteCityAsync(int cityId)
        {
            var existingCity = await _cityRepository.GetCityByIdAsync(cityId);
            if (existingCity == null)
            {
                return false;
            }
            await _cityRepository.DeleteAsync(existingCity);
            return true;
        }

        public Task<CityDto?> GetCityByNameAsync(string cityName, bool includePointsOfInterest)
        {
            throw new NotImplementedException();
        }
    }
}