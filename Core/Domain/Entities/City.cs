using Domain.Common;
using Domain.Entities;

public class City : BaseAuditableEntity
{
    public string Name { get; private set; }
    public string PostalCode { get; private set; }
    public Guid CountryId { get; private set; }
    public virtual Country Country { get; private set; }
    public ICollection<Gym> Gyms { get; private set; }

    private City() { }

    private City(string name, Guid countryId, string postalCode)
    {
        Name = name;
        CountryId = countryId;
        PostalCode = postalCode;
    }

    public static City Create(string name, Guid countryId, string postalCode)
    {
        ValidateInput(name, countryId, postalCode);
        return new City(name, countryId, postalCode);
    }

    public void Update(string name, string postalCode = null)
    {
        if (!string.IsNullOrWhiteSpace(name))
            Name = name;

        if (postalCode != null)
            PostalCode = postalCode;
    }

    private static void ValidateInput(string name, Guid countryId, string postalCode)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("City name is required");

        if (countryId == Guid.Empty)
            throw new ArgumentException("Country ID is required");
    }

    public override string ToString() => $"{Name} ({PostalCode})";
}