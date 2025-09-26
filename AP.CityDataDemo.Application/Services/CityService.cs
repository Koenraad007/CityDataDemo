using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Domain;

namespace AP.CityDataDemo.Application.Services
{

    public class CityService : ICityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<City> Add(City city)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<City>> GetAll(int pageNr, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<City> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<City> Update(City city)
        {
            throw new NotImplementedException();
        }
    }
}