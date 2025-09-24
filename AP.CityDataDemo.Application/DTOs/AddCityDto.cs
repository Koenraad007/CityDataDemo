using System.ComponentModel.DataAnnotations;

namespace AP.CityDataDemo.Application.DTOs;

public class AddCityDto
{
    [Required(ErrorMessage = "Name cannot be empty")]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Population is required")]
    [Range(0, 10000000000, ErrorMessage = "Population cannot be greater than 10,000,000,000")]
    public long Population { get; set; }

    [Required(ErrorMessage = "A country must be selected")]
    [Range(1, int.MaxValue, ErrorMessage = "A country must be selected")]
    public int CountryId { get; set; }
}
