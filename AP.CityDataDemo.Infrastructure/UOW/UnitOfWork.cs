using AP.CityDataDemo.Domain.Interfaces;
using AP.CityDataDemo.Infrastructure.Repositories;
using AP.CityDataDemo.Infrastructure.Data;

namespace AP.CityDataDemo.Infrastructure.UOW;

public class UnitOfWork : IUnitOfWork
{
    private readonly IInMemoryDataStore _dataStore;
    private bool _disposed = false;

    public UnitOfWork(IInMemoryDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public Task<int> SaveChangesAsync()
    {
        return Task.FromResult(0);
    }

    public Task BeginTransactionAsync()
    {
        return Task.CompletedTask;
    }

    public Task CommitTransactionAsync()
    {
        return Task.CompletedTask;
    }

    public Task RollbackTransactionAsync()
    {
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _disposed = true;
        }
    }
}
