namespace AP.CityDataDemo.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ICityRepository Cities { get; }
    ICountryRepository Countries { get; }
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
