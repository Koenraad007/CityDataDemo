namespace AP.CityDataDemo.Application.Interfaces
{
    public interface IUnitOfWork
    {
        public ICitiesRepository CitiesRepository { get; }
        public ICountriesRepository CountriesRepository { get; }
        Task Commit();
    }
}