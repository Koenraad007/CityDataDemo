using System.ComponentModel.DataAnnotations;

namespace AP.CityDataDemo.Application.DTOs;

public class CityDto
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "De naam mag niet leeg zijn")]
    [StringLength(100, ErrorMessage = "De naam mag niet langer zijn dan 100 karakters")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Het aantal inwoners is verplicht")]
    [Range(0, 50000000, ErrorMessage = "Het aantal inwoners mag niet groter zijn dan 50.000.000")]
    public int Population { get; set; }
    
    [Required(ErrorMessage = "Er moet een land gekozen worden")]
    [Range(1, int.MaxValue, ErrorMessage = "Er moet een land gekozen worden")]
    public int CountryId { get; set; }
    
    public string CountryName { get; set; } = string.Empty;
}
