using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configuration
{
    public class TrainerRatingConfiguration : IEntityTypeConfiguration<TrainerRating>
    {
        public void Configure(EntityTypeBuilder<TrainerRating> builder)
        {
            builder.ToTable("TrainerRatings");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Rating)
                .IsRequired();

            builder.Property(r => r.Comment)
                .HasMaxLength(1000);

            builder.HasOne(r => r.Trainer)
                .WithMany(t => t.Ratings)
                .HasForeignKey(r => r.TrainerId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
