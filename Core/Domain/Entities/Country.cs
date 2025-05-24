namespace Domain.Entities
{
    public class Country
    {
        public string Name { get; set; }
        public string CountryCode { get; set; }
        public ICollection<City> Cities { get; private set; } = new List<City>();
    }
}
