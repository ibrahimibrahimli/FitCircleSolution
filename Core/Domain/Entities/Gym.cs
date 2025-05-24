using Domain.Common;

namespace Domain.Entities
{
    public class Gym : BaseAuditableEntity
    {
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public string Address { get; private set; }
        public City City { get; private set; }
        public string PhoneNumber { get; private set; }
        public string? Email { get; private set; }

        public double Latitude { get; private set; } //Map üçün
        public double Longitude { get; private set; }

        public bool IsVipSupported { get; private set; }
        public bool IsCorporatePartner { get; private set; }

        public ICollection<GymFacility> Facilities { get; private set; } = new List<GymFacility>();
        public ICollection<Trainer> Trainers { get; private set; } = new List<Trainer>();



        private Gym() { }

        public Gym(
            string name,
            string address,
            City city,
            string phoneNumber,

            string? description,
            string? email,
            double latitude,
            double longitude,
            bool ısVipSupported,
            bool ısCorporatePartner,
            ICollection<GymFacility> facilities,
            ICollection<Trainer> trainers)
        {
            SetName(name);
            SetAddress(address);
            SetCity(city);
            SetPhoneNumber(phoneNumber);

            UpdateDescription(description);
            UpdateEmail(email);
            Latitude = latitude;
            Longitude = longitude;
        }

        //Behaviors

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("Name is Required");
            if (name.Length > 100) throw new ArgumentException("Name cannot exceed 100 characters");
            Name = name;
        }

        public void SetAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address)) throw new ArgumentNullException("Address is required");
            Address = address;
        }

        public void SetCity(City city)
        {
            if (string.IsNullOrEmpty(city.ToString())) throw new ArgumentNullException("City is required");
            City = city;
        }

        public void SetPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber)) throw new ArgumentNullException("PhoneNumber is Required");
        }

        public void UpdateDescription(string? description) => Description = description;
        public void UpdateEmail(string? email) => Email = email;

    }
}
