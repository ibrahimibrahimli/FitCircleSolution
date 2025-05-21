using Domain.Common;

namespace Domain.Entities
{
    public class City : BaseAuditableEntity
    {
        public string Name { get; set; }
        public string PostalCode { get; set; }
    }
}
