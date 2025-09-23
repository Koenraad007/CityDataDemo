using System.Linq.Expressions;
using AP.CityDataDemo.Application.Interfaces;
using AP.CityDataDemo.Infrastructure.Data;

namespace AP.CityDataDemo.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly IInMemoryDataStore _dataStore;
    protected readonly List<T> _collection;

    public GenericRepository(IInMemoryDataStore dataStore)
    {
        _dataStore = dataStore;
        _collection = GetCollection();
    }

    protected virtual List<T> GetCollection()
    {
        if (typeof(T).Name == "City")
        {
            return (List<T>)(object)_dataStore.Cities;
        }
        if (typeof(T).Name == "Country")
        {
            return (List<T>)(object)_dataStore.Countries;
        }
        throw new InvalidOperationException($"No collection configured for type {typeof(T).Name}");
    }

    public virtual Task<IEnumerable<T>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<T>>(_collection);
    }

    public virtual Task<T?> GetByIdAsync(int id)
    {
        // Since we removed IBaseEntity, we need to use reflection to find Id property
        var entity = _collection.FirstOrDefault(e => 
        {
            var idProperty = e.GetType().GetProperty("Id");
            return idProperty != null && (int)idProperty.GetValue(e)! == id;
        });
        return Task.FromResult(entity);
    }

    public virtual Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        var result = _collection.AsQueryable().Where(predicate);
        return Task.FromResult<IEnumerable<T>>(result);
    }

    public virtual Task AddAsync(T entity)
    {
        _collection.Add(entity);
        return Task.CompletedTask;
    }

    public virtual Task AddRangeAsync(IEnumerable<T> entities)
    {
        _collection.AddRange(entities);
        return Task.CompletedTask;
    }

    public virtual Task UpdateAsync(T entity)
    {
        var idProperty = entity.GetType().GetProperty("Id");
        if (idProperty == null) return Task.CompletedTask;
        
        var entityId = (int)idProperty.GetValue(entity)!;
        var existingEntity = _collection.FirstOrDefault(e => 
        {
            var existingIdProperty = e.GetType().GetProperty("Id");
            return existingIdProperty != null && (int)existingIdProperty.GetValue(e)! == entityId;
        });
        
        if (existingEntity != null)
        {
            var index = _collection.IndexOf(existingEntity);
            _collection[index] = entity;
        }
        return Task.CompletedTask;
    }

    public virtual Task DeleteAsync(T entity)
    {
        _collection.Remove(entity);
        return Task.CompletedTask;
    }

    public virtual Task DeleteByIdAsync(int id)
    {
        var entity = _collection.FirstOrDefault(e => 
        {
            var idProperty = e.GetType().GetProperty("Id");
            return idProperty != null && (int)idProperty.GetValue(e)! == id;
        });
        
        if (entity != null)
        {
            _collection.Remove(entity);
        }
        return Task.CompletedTask;
    }
}
