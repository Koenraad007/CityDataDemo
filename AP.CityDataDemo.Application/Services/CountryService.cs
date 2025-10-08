using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AP.CityDataDemo.Application.DTOs;
using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Domain;
using AutoMapper;

namespace AP.CityDataDemo.Application.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryService(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository ?? throw new ArgumentNullException(nameof(countryRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<CountryDto>> GetCountriesAsync()
        {
            var countries = await _countryRepository.GetAllCountriesAsync();
            return _mapper.Map<IEnumerable<CountryDto>>(countries);
        }

        public async Task<CountryDto?> GetCountryByIdAsync(int countryId, bool includeCities)
        {
            var country = await _countryRepository.GetCountryByIdAsync(countryId);
            return country == null ? null : _mapper.Map<CountryDto>(country);
        }

        public async Task<CountryDto?> GetCountryByNameAsync(string countryName, bool includeCities)
        {
            if (string.IsNullOrWhiteSpace(countryName))
                return null;

            var countries = await _countryRepository.GetAllCountriesAsync();
            var country = countries.FirstOrDefault(c => string.Equals(c.Name, countryName, StringComparison.OrdinalIgnoreCase));
            return country == null ? null : _mapper.Map<CountryDto>(country);
        }

        public async Task<CountryDto> CreateCountryAsync(CountryDto countryDto)
        {
            var countryEntity = _mapper.Map<Country>(countryDto);
            await _countryRepository.AddCountryAsync(countryEntity);
            return _mapper.Map<CountryDto>(countryEntity);
        }

        public async Task<bool> UpdateCountryAsync(int countryId, CountryDto countryDto)
        {
            var existing = await _countryRepository.GetCountryByIdAsync(countryId);
            if (existing == null)
            {
                return false;
            }

            _mapper.Map(countryDto, existing);
            await _countryRepository.UpdateCountryAsync(existing);
            return true;
        }

        public async Task<bool> DeleteCountryAsync(int countryId)
        {
            var existing = await _countryRepository.GetCountryByIdAsync(countryId);
            if (existing == null)
            {
                return false;
            }

            await _countryRepository.DeleteCountryAsync(existing);
            return true;
        }
    }
}
