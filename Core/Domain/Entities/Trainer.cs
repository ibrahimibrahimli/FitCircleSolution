using Domain.Common;

namespace Domain.Entities
{
    public class Trainer : BaseAuditableEntity
    {
        private Trainer() { }

        private Trainer(
            string firstname,
            string lastname,
            string email,
            string phoneNumber,
            string bio,
            Guid gymId,
            string? profileImageUrl,
            string? coverImageUrl,
            string instagramHandle)
        {
            Firstname = firstname;
            Lastname = lastname;
            Email = email;
            PhoneNumber = phoneNumber;
            Bio = bio;
            GymId = gymId;
            ProfileImageUrl = profileImageUrl;
            CoverImageUrl = coverImageUrl;
            InstagramHandle = instagramHandle;
        }

        public string Firstname { get; private set; }
        public string Lastname { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Bio { get; private set; }

        public Guid GymId { get; private set; }
        public Gym Gym { get; private set; } // Lazy loading istifadə edəcəyik deyə virtual etməyə ehtiyac yoxdur

        public string? ProfileImageUrl { get; private set; }
        public string? CoverImageUrl { get; private set; }
        public string InstagramHandle { get; private set; }

        private readonly List<TrainerRating> _ratings = new();
        public IReadOnlyCollection<TrainerRating> Ratings => _ratings.AsReadOnly();

        public static Trainer Create(
            string firstname,
            string lastname,
            string email,
            string phoneNumber,
            string bio,
            Guid gymId,
            string? profileImageUrl,
            string? coverImageUrl,
            string instagramHandle)
        {
            // Burada sadə validation-lar da əlavə etmək mümkündür (məs: email forması, telefon uzunluğu və s.)
            return new Trainer(firstname, lastname, email, phoneNumber, bio, gymId, profileImageUrl, coverImageUrl, instagramHandle);
        }

        public void UpdateProfile(
            string firstname,
            string lastname,
            string phoneNumber,
            string bio,
            string? profileImageUrl,
            string? coverImageUrl,
            string instagramHandle)
        {
            Firstname = firstname;
            Lastname = lastname;
            PhoneNumber = phoneNumber;
            Bio = bio;
            ProfileImageUrl = profileImageUrl;
            CoverImageUrl = coverImageUrl;
            InstagramHandle = instagramHandle;
        }

        public void AddRating(TrainerRating rating)
        {
            if (rating == null) throw new ArgumentNullException(nameof(rating));
            _ratings.Add(rating);
        }
    }
}
