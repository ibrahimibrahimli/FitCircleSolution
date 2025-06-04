using Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configuration
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            // Table configuration
            builder.ToTable("AspNetUsers");

            // Basic properties
            builder.Property(u => u.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.LastName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.Email)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(u => u.NormalizedEmail)
                .HasMaxLength(256);

            builder.Property(u => u.UserName)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(u => u.NormalizedUserName)
                .HasMaxLength(256);

            builder.Property(u => u.PhoneNumber)
                .HasMaxLength(20);

            // Profile properties
            builder.Property(u => u.ProfileImageUrl)
                .HasMaxLength(500);

            builder.Property(u => u.DateOfBirth)
                .HasColumnType("date");

            builder.Property(u => u.Gender)
                .HasConversion<int>()
                .HasColumnName("GenderId");

            builder.Property(u => u.Height)
                .HasColumnType("decimal(5,2)")
                .HasComment("Height in centimeters");

            builder.Property(u => u.Weight)
                .HasColumnType("decimal(5,2)")
                .HasComment("Weight in kilograms");

            builder.Property(u => u.Address)
                .HasMaxLength(500);

            // Emergency contact
            builder.Property(u => u.EmergencyContact)
                .HasMaxLength(200);

            builder.Property(u => u.EmergencyContactPhone)
                .HasMaxLength(20);

            // Timestamp properties
            builder.Property(u => u.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(u => u.LastLoginAt);

            // Status properties
            builder.Property(u => u.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(u => u.DeactivatedAt);

            builder.Property(u => u.DeactivationReason)
                .HasMaxLength(500);

            // Computed properties (ignored because they're calculated)
            builder.Ignore(u => u.FullName);
            builder.Ignore(u => u.BMI);
            builder.Ignore(u => u.IsProfileComplete);
            builder.Ignore(u => u.Age);
            builder.Ignore(u => u.ActiveSubscription);
            builder.Ignore(u => u.HasActiveSubscription);
            builder.Ignore(u => u.CurrentSubscriptionType);
            builder.Ignore(u => u.AssignedTrainer);

            

            // Navigation properties configuration
            //builder.HasMany(u => u.TrainerRatings)
            //    .WithOne(tr => tr.Member)
            //    .HasForeignKey(tr => tr.MemberId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //builder.HasMany(u => u.Subscriptions)
            //    .WithOne(s => s.Member)
            //    .HasForeignKey(s => s.MemberId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //builder.HasMany(u => u.Payments)
            //    .WithOne(p => p.Member)
            //    .HasForeignKey(p => p.MemberId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //builder.HasMany(u => u.DietPlans)
            //    .WithOne(dp => dp.Member)
            //    .HasForeignKey(dp => dp.MemberId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //builder.HasMany(u => u.WorkoutPlans)
            //    .WithOne(wp => wp.Member)
            //    .HasForeignKey(wp => wp.MemberId)
            //    .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
