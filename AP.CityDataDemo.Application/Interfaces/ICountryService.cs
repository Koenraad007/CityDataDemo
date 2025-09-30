using AP.CityDataDemo.Domain;

namespace AP.CityDataDemo.Application.Interfaces
{
    public interface ICountryService
    {
        public Task<IEnumerable<Country>> GetAll(int pageNr, int pageSize);
        public Task<Country> GetById(int id);
        public Task<Country> Add(Country country);
        public Task Delete(int id);
        public Task<Country> Update(Country country);
    }
}