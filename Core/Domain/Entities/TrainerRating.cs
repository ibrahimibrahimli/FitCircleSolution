using Domain.Common;

namespace Domain.Entities
{
    public class TrainerRating : BaseAuditableEntity
    {
        private TrainerRating() { }

        private TrainerRating(Guid trainerId, Guid userId, int rating, string? comment)
        {
            TrainerId = trainerId;
            UserId = userId;
            Rating = rating;
            Comment = comment;
        }

        public Guid TrainerId { get; private set; }
        public Trainer Trainer { get; private set; }

        public Guid UserId { get; private set; }

        public int Rating { get; private set; }
        public string? Comment { get; private set; }

        public static TrainerRating Create(Guid trainerId, Guid userId, int rating, string? comment = null)
        {
            if (rating < 1 || rating > 5)
                throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 1 and 5");

            return new TrainerRating(trainerId, userId, rating, comment);
        }

        public void UpdateRating(int rating, string? comment = null)
        {
            if (rating < 1 || rating > 5)
                throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 1 and 5");

            Rating = rating;
            Comment = comment;
        }
    }
}
