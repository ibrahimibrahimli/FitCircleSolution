using Domain.Common;

namespace Domain.Entities
{
    public class Gym : BaseAuditableEntity
    {
        private readonly List<GymFacility> _facilities = new();
        private readonly List<Trainer> _trainers = new();

        private Gym() { } // ORM üçün

        private Gym(
            string name,
            string? description,
            string address,
            City city,
            string phoneNumber,
            string? email,
            double latitude,
            double longitude,
            bool isVipSupported,
            bool isCorporatePartner)
        {
            Name = name;
            Description = description;
            Address = address;
            City = city;
            PhoneNumber = phoneNumber;
            Email = email;
            Latitude = latitude;
            Longitude = longitude;
            IsVipSupported = isVipSupported;
            IsCorporatePartner = isCorporatePartner;
        }

        public string Name { get; private set; }
        public string? Description { get; private set; }
        public string Address { get; private set; }
        public City City { get; private set; }
        public string PhoneNumber { get; private set; }
        public string? Email { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public bool IsVipSupported { get; private set; }
        public bool IsCorporatePartner { get; private set; }

        public IReadOnlyCollection<GymFacility> Facilities => _facilities.AsReadOnly();
        public IReadOnlyCollection<Trainer> Trainers => _trainers.AsReadOnly();

        public static Gym Create(
            string name,
            string? description,
            string address,
            City city,
            string phoneNumber,
            string? email,
            double latitude,
            double longitude,
            bool isVipSupported,
            bool isCorporatePartner)
        {
            return new(name, description, address, city, phoneNumber, email, latitude, longitude, isVipSupported, isCorporatePartner);
        }

        public void UpdateInfo(
            string name,
            string? description,
            string address,
            City city,
            string phoneNumber,
            string? email,
            double latitude,
            double longitude,
            bool isVipSupported,
            bool isCorporatePartner)
        {
            Name = name;
            Description = description;
            Address = address;
            City = city;
            PhoneNumber = phoneNumber;
            Email = email;
            Latitude = latitude;
            Longitude = longitude;
            IsVipSupported = isVipSupported;
            IsCorporatePartner = isCorporatePartner;
        }

        public void AddFacility(GymFacility facility)
        {
            _facilities.Add(facility);
        }

        public void AddTrainer(Trainer trainer)
        {
            _trainers.Add(trainer);
        }
    }
}
