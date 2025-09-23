using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Domain;
using AP.CityDataDemo.Infrastructure.Contexts;

namespace AP.CityDataDemo.Infrastructure.Repositories
{
    public class CitiesRepository : GenericRepository<City>, ICitiesRepository
    {
        private readonly CityDataDemoContext context;

        public CitiesRepository(CityDataDemoContext context) : base(context)
        {
            this.context = context;
        }
    }
}