using AP.CityDataDemo.Domain.Interfaces;

namespace AP.CityDataDemo.Domain.Entities;

public class City : IBaseEntity
{
    public int Id { get; set; }
    public string Name { get; private set; }
    public int Population { get; private set; }
    public int CountryId { get; private set; }
    public Country Country { get; private set; } = null!;

    public City(string name, int population, int countryId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("City name cannot be null or empty.", nameof(name));
        if (population < 0)
            throw new ArgumentException("Population cannot be negative.", nameof(population));
        if (countryId <= 0)
            throw new ArgumentException("CountryId must be a positive value.", nameof(countryId));

        Name = name;
        Population = population;
        CountryId = countryId;
    }

    public void UpdatePopulation(int newPopulation)
    {
        if (newPopulation < 0)
            throw new ArgumentException("Population cannot be negative.", nameof(newPopulation));
        
        Population = newPopulation;
    }
    
    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("City name cannot be null or empty.", nameof(newName));
        
        Name = newName;
    }
}
