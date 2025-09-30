using AP.CityDataDemo.Domain;
using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AP.CityDataDemo.Infrastructure.Repositories
{
    public class CityRepository : GenericRepository<City>, ICityRepository
    {

        public CityRepository(CityDataDemoContext ctx) : base(ctx)
        {
        }

        public async Task<int> GetCountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public async Task<IEnumerable<City>> GetAllAsync(bool sortByName, bool descending)
        {
            IQueryable<City> query = _dbSet.AsNoTracking();
            if (sortByName)
            {
                query = descending ? query.OrderByDescending(c => c.Name) : query.OrderBy(c => c.Name);
            }
            else
            {
                query = descending ? query.OrderByDescending(c => c.Population) : query.OrderBy(c => c.Population);
            }
            return await query.ToListAsync();
        }

        public Task<City?> GetCityByIdAsync(int id)
        {
            return GetByIdAsync(id);
        }

        public Task AddCityAsync(City city)
        {
            return AddAsync(city);
        }

        public Task AddCitiesAsync(IEnumerable<City> cities)
        {
            return AddRangeAsync(cities);
        }

        public Task<bool> UpdateCityAsync(City city)
        {
            return UpdateAsync(city);
        }

        public Task DeleteCityAsync(City city)
        {
            return DeleteAsync(city);
        }

        public Task<bool> DeleteCityByIdAsync(int id)
        {
            return DeleteByIdAsync(id);
        }

        public async Task<bool> CityNameExistsAsync(string name)
        {
            return await _dbSet.AnyAsync(c => c.Name.ToLower() == name.ToLower());
        }

    }
}
