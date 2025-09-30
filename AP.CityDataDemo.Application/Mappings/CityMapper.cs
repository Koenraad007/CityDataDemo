using AP.CityDataDemo.Application.DTOs;
using AP.CityDataDemo.Domain;

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
            CountryId = entity.CountryId,
            CountryName = entity.Country?.Name ?? "N/A"
        };
    }

    public static City ToEntity(this CityDto dto)
    {
        return new City() { Name = dto.Name, Population = (int)dto.Population, CountryId = dto.CountryId };
    }
}
