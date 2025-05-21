using Domain.Common;

namespace Domain.Entities
{
    public class TrainerRating : BaseAuditableEntity
    {
        public Guid TrainerId { get; set; }
        public Trainer Trainer { get; set; }
        public Guid UserId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
