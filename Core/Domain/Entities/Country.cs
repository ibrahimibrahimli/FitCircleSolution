using Domain.Common;

public class Country : BaseAuditableEntity
{
    private readonly List<City> _cities = new();

    private Country() { }

    private Country(string name, string countryCode)
    {
        Name = name;
        CountryCode = countryCode?.ToUpperInvariant();
    }

    public string Name { get; private set; }
    public string CountryCode { get; private set; }
    public IReadOnlyCollection<City> Cities => _cities.AsReadOnly();

    public static Country Create(string name, string countryCode)
    {
        ValidateInput(name, countryCode);
        return new Country(name, countryCode);
    }

    public void Update(string name, string countryCode)
    {
        ValidateInput(name, countryCode);
        Name = name;
        CountryCode = countryCode?.ToUpperInvariant();
    }

    public void AddCity(City city)
    {
        if (city == null)
            throw new ArgumentNullException(nameof(city));

        if (city.CountryId != Id)
            throw new InvalidOperationException("City must belong to this country");

        if (_cities.Any(c => c.Id == city.Id))
            throw new InvalidOperationException("City already exists");

        _cities.Add(city);
    }

    public void RemoveCity(Guid cityId)
    {
        var city = _cities.FirstOrDefault(c => c.Id == cityId);
        if (city != null)
            _cities.Remove(city);
    }

    private static void ValidateInput(string name, string countryCode)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Country name is required");

        if (string.IsNullOrWhiteSpace(countryCode))
            throw new ArgumentException("Country code is required");

        if (countryCode.Length is not (2 or 3))
            throw new ArgumentException("Country code must be 2 or 3 characters (ISO standard)");
    }

    public override string ToString() => $"{Name} ({CountryCode})";
}