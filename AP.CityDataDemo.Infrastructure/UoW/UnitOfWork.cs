namespace AP.CityDataDemo.Infrastructure.UoW
{
    using AP.CityDataDemo.Application.Interfaces;
    using System.Threading.Tasks;
    using AP.CityDataDemo.Infrastructure.Contexts;

    public class UnitOfWork : IUnitOfWork
    {
        private readonly CityDataDemoContext _context;
        private readonly ICitiesRepository _citiesRepository;
        private readonly ICountriesRepository _countriesRepository;

        public UnitOfWork(CityDataDemoContext context, ICitiesRepository citiesRepository, ICountriesRepository countriesRepository)
        {
            _context = context;
            _citiesRepository = citiesRepository;
            _countriesRepository = countriesRepository;
        }

        public ICitiesRepository CitiesRepository => _citiesRepository;

        public ICountriesRepository CountriesRepository => _countriesRepository;

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
    }
}