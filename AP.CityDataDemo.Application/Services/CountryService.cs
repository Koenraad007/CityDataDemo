using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Domain;

namespace AP.CityDataDemo.Application.Services
{
    public class CountryService : ICountryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CountryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Country> Add(Country country)
        {
            var existingCountry = await _unitOfWork.CountriesRepository.GetByIdAsync(country.Id);
            if (existingCountry != null)
            {
                throw new Exception("Country with the given ID already exists.");
            }

            await _unitOfWork.CountriesRepository.AddAsync(country);
            await _unitOfWork.Commit();
            return country;
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Country>> GetAll(int pageNr, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<Country> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Country> Update(Country country)
        {
            throw new NotImplementedException();
        }
    }
}