using AP.CityDataDemo.Domain.Interfaces;

namespace AP.CityDataDemo.Application.Services;

public abstract class GenericService<TDto, TEntity> : IGenericService<TDto, TEntity>
    where TEntity : class, IBaseEntity
    where TDto : class
{
    protected readonly IUnitOfWork _unitOfWork;
    protected readonly IGenericRepository<TEntity> _repository;

    protected GenericService(IUnitOfWork unitOfWork, IGenericRepository<TEntity> repository)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
    }

    public virtual async Task<IEnumerable<TDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return entities.Select(MapToDto);
    }

    public virtual async Task<TDto?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity == null ? null : MapToDto(entity);
    }

    public virtual async Task<TDto> CreateAsync(TDto dto)
    {
        var entity = MapToEntity(dto);
        await _repository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return MapToDto(entity);
    }

    public virtual async Task<TDto> UpdateAsync(int id, TDto dto)
    {
        var entity = MapToEntity(dto);
        entity.Id = id;
        await _repository.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return MapToDto(entity);
    }

    public virtual async Task DeleteAsync(int id)
    {
        await _repository.DeleteByIdAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    protected abstract TDto MapToDto(TEntity entity);
    protected abstract TEntity MapToEntity(TDto dto);
}
