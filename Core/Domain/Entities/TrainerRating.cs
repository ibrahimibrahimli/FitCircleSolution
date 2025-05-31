using Domain.Common;
using Domain.Identity;

namespace Domain.Entities
{
    public class TrainerRating : BaseAuditableEntity
    {
        public const int MinRating = 1;
        public const int MaxRating = 5;
        public const int MaxCommentLength = 500;

        private TrainerRating() { }

        private TrainerRating(Guid trainerId, Guid userId, int rating, string? comment)
        {
            TrainerId = trainerId;
            UserId = userId;
            Rating = rating;
            Comment = comment?.Trim();
            IsActive = true;
        }

        // Core Properties
        public Guid TrainerId { get; private set; }
        public Trainer Trainer { get; private set; }
        public Guid UserId { get; private set; }
        public ApplicationUser User { get; private set; } // User navigation property əlavə etdim
        public int Rating { get; private set; }
        public string? Comment { get; private set; }
        public bool IsActive { get; private set; }

        // Additional Properties
        public DateTime? EditedAt { get; private set; }
        public bool IsEdited => EditedAt.HasValue;
        public bool HasComment => !string.IsNullOrWhiteSpace(Comment);

        // Factory Method
        public static TrainerRating Create(Guid trainerId, Guid userId, int rating, string? comment = null)
        {
            ValidateInput(trainerId, userId, rating, comment);
            return new TrainerRating(trainerId, userId, rating, comment);
        }

        // Update Methods
        public void UpdateRating(int rating, string? comment = null)
        {
            ValidateRating(rating);
            ValidateComment(comment);

            var oldRating = Rating;
            var oldComment = Comment;

            Rating = rating;
            Comment = comment?.Trim();

            // Mark as edited if there was a change
            if (oldRating != rating || oldComment != Comment)
            {
                EditedAt = DateTime.UtcNow;
            }
        }

        public void UpdateComment(string? comment)
        {
            ValidateComment(comment);

            var oldComment = Comment;
            Comment = comment?.Trim();

            // Mark as edited if comment changed
            if (oldComment != Comment)
            {
                EditedAt = DateTime.UtcNow;
            }
        }

        // Status Management
        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
            EditedAt = DateTime.UtcNow;
        }

        // Business Logic Methods
        public bool IsHighRating() => Rating >= 4;
        public bool IsLowRating() => Rating <= 2;
        public bool IsMediumRating() => Rating == 3;

        public string GetRatingDescription() => Rating switch
        {
            1 => "Very Poor",
            2 => "Poor",
            3 => "Average",
            4 => "Good",
            5 => "Excellent",
            _ => "Unknown"
        };

        public string GetRatingEmoji() => Rating switch
        {
            1 => "😠",
            2 => "😞",
            3 => "😐",
            4 => "😊",
            5 => "😍",
            _ => "❓"
        };

        public bool CanBeEditedBy(Guid userId)
        {
            return UserId == userId && IsActive;
        }

        public TimeSpan GetTimeSinceCreated()
        {
            return DateTime.UtcNow - CreatedAt;
        }

        public TimeSpan? GetTimeSinceEdited()
        {
            return EditedAt.HasValue ? DateTime.UtcNow - EditedAt.Value : null;
        }

        public bool IsRecentlyCreated(TimeSpan timespan)
        {
            return GetTimeSinceCreated() <= timespan;
        }

        public bool IsRecentlyEdited(TimeSpan timespan)
        {
            var timeSinceEdited = GetTimeSinceEdited();
            return timeSinceEdited.HasValue && timeSinceEdited.Value <= timespan;
        }

        // Validation Methods
        private static void ValidateInput(Guid trainerId, Guid userId, int rating, string? comment)
        {
            if (trainerId == Guid.Empty)
                throw new ArgumentException("Trainer ID is required", nameof(trainerId));

            if (userId == Guid.Empty)
                throw new ArgumentException("User ID is required", nameof(userId));

            if (trainerId == userId)
                throw new ArgumentException("Trainer cannot rate themselves");

            ValidateRating(rating);
            ValidateComment(comment);
        }

        private static void ValidateRating(int rating)
        {
            if (rating < MinRating || rating > MaxRating)
                throw new ArgumentOutOfRangeException(nameof(rating),
                    $"Rating must be between {MinRating} and {MaxRating}");
        }

        private static void ValidateComment(string? comment)
        {
            if (!string.IsNullOrEmpty(comment))
            {
                if (comment.Length > MaxCommentLength)
                    throw new ArgumentException(
                        $"Comment cannot exceed {MaxCommentLength} characters", nameof(comment));

                // Check for inappropriate content (basic validation)
                if (ContainsInappropriateContent(comment))
                    throw new ArgumentException("Comment contains inappropriate content", nameof(comment));
            }
        }

        private static bool ContainsInappropriateContent(string comment)
        {
            // Basic inappropriate content check
            // Bu real proyektdə daha sophisticated olmalıdır
            var inappropriateWords = new[] { "spam", "fake", "bot" };
            return inappropriateWords.Any(word =>
                comment.Contains(word, StringComparison.OrdinalIgnoreCase));
        }

        // Comparison Methods
        public int CompareTo(TrainerRating other)
        {
            if (other == null) return 1;

            // Sort by rating (descending), then by creation date (descending)
            var ratingComparison = other.Rating.CompareTo(Rating);
            return ratingComparison != 0 ? ratingComparison : other.CreatedAt.CompareTo(CreatedAt);
        }

        public override string ToString()
        {
            var ratingText = $"{GetRatingEmoji()} {Rating}/{MaxRating}";
            var commentText = HasComment ? $" - \"{Comment}\"" : "";
            var editedText = IsEdited ? " (edited)" : "";
            return $"{ratingText}{commentText}{editedText}";
        }

        public override bool Equals(object obj) =>
            obj is TrainerRating other && Id == other.Id;

        public override int GetHashCode() => Id.GetHashCode();
    }
}