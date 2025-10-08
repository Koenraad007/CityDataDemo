using System.Linq.Expressions;
using AP.CityDataDemo.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AP.CityDataDemo.Infrastructure.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DbSet<T> _dbSet;

        protected GenericRepository(DbContext context)
        {
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync<TKey>(
            int pageNr,
            int pageSz,
            Expression<Func<T, TKey>> orderBy,
            bool ascending = true,
            CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();

            query = ascending
            ? query.OrderBy(orderBy)
            : query.OrderByDescending(orderBy);

            return await query
            .Skip((pageNr - 1) * pageSz)
            .Take(pageSz)
            .ToListAsync(cancellationToken);
        }

        public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddRangeAsync(entities, cancellationToken);
        }

        public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<bool> DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _dbSet.FindAsync(new object[] { id }, cancellationToken);
            if (entity == null)
                return false;
            _dbSet.Remove(entity);
            return true;
        }
    }
}