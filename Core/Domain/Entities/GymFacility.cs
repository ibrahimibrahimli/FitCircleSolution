using Domain.Common;

namespace Domain.Entities
{
    public class GymFacility : BaseAuditableEntity
    {
        public Guid GymId { get; set; }
        public Gym Gym { get; set; }

        public GymType Type { get; set; }
    }
}
