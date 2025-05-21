using Domain.Common;

namespace Domain.Entities
{
    public class Trainer : BaseAuditableEntity
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Bio { get; set; }

        public string? ProfileImageUrl { get; set; }
        public string? CoverImageUrl { get; set; }
        public string InstagramHandle { get; set; }

        public ICollection<TrainerRating> Ratings { get; set; } = new List<TrainerRating>();

    }
}
