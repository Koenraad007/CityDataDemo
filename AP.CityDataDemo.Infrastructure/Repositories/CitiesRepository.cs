using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Domain;
using AP.CityDataDemo.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AP.CityDataDemo.Infrastructure.Repositories
{
    public class CitiesRepository : GenericRepository<City>, ICitiesRepository
    {
        private readonly CityDataDemoContext context;

        public CitiesRepository(CityDataDemoContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<City>> GetAll(int pageNr, int pageSize)
        {
            return await context.Cities
                .Skip((pageNr - 1) * pageSize)
                .Take(pageSize).Include(c => c.Country)
                .ToListAsync();
        }


    }
}