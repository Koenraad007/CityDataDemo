namespace AP.CityDataDemo.Application.Interfaces
{
    public interface IGenericRepository<T>
    {
        Task<IEnumerable<T>> GetAll(int pageNr, int pageSize);

        Task<T> GetById(int id);
        Task<T> Create(T newStore);

        T Update(T modifiedStore);
        void Delete(T store);
    }
}