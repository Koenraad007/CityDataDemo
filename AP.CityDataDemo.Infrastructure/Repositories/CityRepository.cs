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

        public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.CountAsync(cancellationToken);
        }

        public async Task<bool> CityNameExistsAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking().AnyAsync(c => c.Name == name, cancellationToken);
        }

    }
}
