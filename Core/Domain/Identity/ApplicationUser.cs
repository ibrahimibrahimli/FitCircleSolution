using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Domain.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string Firstname { get; private set; }
        public string Lastname { get; private set; }
        public string? ProfileImageUrl { get; private set; }

        // Navigational Properties
        public ICollection<TrainerRating> TrainerRatings { get; private set; } = new List<TrainerRating>();
        public ICollection<UserSubscription> Subscriptions { get; private set; } = new List<UserSubscription>();
        public ICollection<Payment> Payments { get; private set; } = new List<Payment>();
        public ICollection<DietPlan> DietPlans { get; private set; } = new List<DietPlan>();
        public ICollection<WorkoutPlan> WorkoutPlans { get; private set; } = new List<WorkoutPlan>();

        private ApplicationUser() { }

        public ApplicationUser(string firstname, string lastname, string email, string phoneNumber)
        {
            Firstname = firstname;
            Lastname = lastname;
            Email = email;
            PhoneNumber = phoneNumber;
            UserName = email;
        }

        public void UpdateProfile(string firstname, string lastname, string? profileImageUrl)
        {
            Firstname = firstname;
            Lastname = lastname;
            ProfileImageUrl = profileImageUrl;
        }
    }
}
