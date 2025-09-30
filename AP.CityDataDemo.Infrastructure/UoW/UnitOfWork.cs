namespace AP.CityDataDemo.Infrastructure.UOW
{
    using AP.CityDataDemo.Application.Interfaces;
    using System.Threading.Tasks;
    using AP.CityDataDemo.Infrastructure.Contexts;

    public class UnitOfWork : IUnitOfWork
    {
        private readonly CityDataDemoContext _context;
        private readonly ICityRepository _citiesRepository;
        private readonly ICountryRepository _countriesRepository;

        public UnitOfWork(CityDataDemoContext context, ICityRepository citiesRepository, ICountryRepository countriesRepository)
        {
            _context = context;
            _citiesRepository = citiesRepository;
            _countriesRepository = countriesRepository;
        }

        public ICityRepository CitiesRepository => _citiesRepository;

        public ICountryRepository CountriesRepository => _countriesRepository;

        public async Task Commit(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}