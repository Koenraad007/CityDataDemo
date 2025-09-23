using AP.CityDataDemo.Domain.Interfaces;

namespace AP.CityDataDemo.Application.Services;

public interface IGenericService<TDto, TEntity> 
    where TEntity : class, IBaseEntity
    where TDto : class
{
    Task<IEnumerable<TDto>> GetAllAsync();
    Task<TDto?> GetByIdAsync(int id);
    Task<TDto> CreateAsync(TDto dto);
    Task<TDto> UpdateAsync(int id, TDto dto);
    Task DeleteAsync(int id);
}
