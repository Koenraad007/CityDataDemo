using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Domain;
using AP.CityDataDemo.Infrastructure.Contexts;

namespace AP.CityDataDemo.Infrastructure.Repositories
{
    public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
    {
        private readonly CityDataDemoContext context;

        public CountriesRepository(CityDataDemoContext context) : base(context)
        {
            this.context = context;
        }
    }
}