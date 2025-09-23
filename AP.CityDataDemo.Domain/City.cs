namespace AP.CityDataDemo.Domain
{
    public class City
    {
        public string Name { get; set; }
        public int Population { get; set; }
        public Country Country { get; set; }
    }
}