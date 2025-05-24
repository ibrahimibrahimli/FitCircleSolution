using Domain.Common;
using Domain.Entities;

public class City : BaseAuditableEntity
{
    public string Name { get; private set; }
    public string  PostalCode { get; set; }
    public Guid CountryId { get; private set; }
    public Country Country { get; private set; }

    private City() { }

    private City(string name, Guid countryId, string postalCode)
    {
        Name = name;
        CountryId = countryId;
        PostalCode = postalCode;
    }

    public static City Create(string name, Guid countryId, string postalCode)
    {
        return new City(name, countryId, postalCode);
    }

    public void Update(string name)
    {
        Name = name;
    }
}
