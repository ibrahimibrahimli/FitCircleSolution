using Domain.Common;

namespace Domain.Entities;

public class Country : BaseAuditableEntity
{
    private Country() { }

    private Country(string name, string countryCode)
    {
        Name = name;
        CountryCode = countryCode;
        _cities = new List<City>(); // Fix for IDE0028: Simplified collection initialization
    }

    public string Name { get; private set; }
    public string CountryCode { get; private set; }

    private readonly List<City> _cities = new(); // Fix for IDE0028: Simplified collection initialization
    public IReadOnlyCollection<City> Cities => _cities.AsReadOnly();

    public static Country Create(string name, string countryCode)
    {
        return new Country(name, countryCode);
    }

    public void AddCity(City city)
    {
        if (city == null) throw new ArgumentNullException(nameof(city));
        if (city.CountryId != Id) throw new InvalidOperationException("City.CountryId must match Country.Id");

        _cities.Add(city);
    }

    public void Update(string name, string countryCode)
    {
        Name = name;
        CountryCode = countryCode;
    }
}
