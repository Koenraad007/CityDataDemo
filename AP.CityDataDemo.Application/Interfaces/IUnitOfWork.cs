namespace AP.CityDataDemo.Application.Interfaces
{
    public interface IUnitOfWork
    {
        public ICityRepository CitiesRepository { get; }
        public ICountryRepository CountriesRepository { get; }
        Task Commit();
    }
}