namespace AP.CityDataDemo.Domain.Entities;

public class City
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Population { get; set; }
    public int CountryId { get; set; }
    public Country Country { get; set; } = null!;
}
