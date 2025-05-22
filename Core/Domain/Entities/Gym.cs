using Domain.Common;

namespace Domain.Entities
{
    public class Gym : BaseAuditableEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; } 
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }

        public double Latitude { get; set; } //Map üçün
        public double Longitude { get; set; }

        public bool IsVipSupported { get; set; }
        public bool IsCorporatePartner { get; set; }

        public ICollection<GymFacility> Facilities { get; set; } = new List<GymFacility>();
        public ICollection<Trainer> Trainers { get; set; } = new List<Trainer>();
    }
}
