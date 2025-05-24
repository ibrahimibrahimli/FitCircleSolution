using Domain.Common;

namespace Domain.Entities
{
    public class City : BaseAuditableEntity
    {
        public string Name { get; set; }
        public string PostalCode { get; set; }
        public Country Country { get; set; }
        public Guid CountryId { get; set; }
        public ICollection<Gym> Trainers { get; private set; } = new List<Gym>();
    }
}
