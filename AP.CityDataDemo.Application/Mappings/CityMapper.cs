using AP.CityDataDemo.Application.DTOs;
using AP.CityDataDemo.Domain.Entities;

namespace AP.CityDataDemo.Application.Mappings;

public static class CityMapper
{
    public static CityDto ToDto(this City entity)
    {
        return new CityDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Population = entity.Population,
            CountryName = entity.Country?.Name ?? "N/A"
        };
    }

    public static City ToEntity(this CityDto dto)
    {
        return new City(dto.Name, dto.Population, 1);
    }
}
