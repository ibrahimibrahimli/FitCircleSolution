using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class GymFacility : BaseAuditableEntity
{
    private GymFacility() { } // ORM üçün

    private GymFacility(string name, string description, GymType type, Guid gymId)
    {
        Name = name;
        Description = description;
        Type = type;
        GymId = gymId;
    }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public GymType Type { get; private set; }

    public Guid GymId { get; private set; }
    public Gym Gym { get; private set; }

    public static GymFacility Create(string name, string description, GymType type, Guid gymId)
    {
        return new GymFacility(name, description, type, gymId);
    }

    public void Update(string name, string description, GymType type)
    {
        Name = name;
        Description = description;
        Type = type;
    }
}
