using Domain.Common;

namespace Domain.Entities
{
    public class GymFacility : BaseAuditableEntity
    {
        public string Name { get; set; }
        public Guid GymId { get; set; }
        public Gym Gym { get; set; }
        public string Description { get; set; }

        public GymType Type { get; set; }
    }
}
