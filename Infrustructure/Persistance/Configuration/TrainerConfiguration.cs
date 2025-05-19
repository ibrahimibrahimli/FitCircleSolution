using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configuration
{
    public class TrainerConfiguration : IEntityTypeConfiguration<Trainer>
    {
        public void Configure(EntityTypeBuilder<Trainer> builder)
        {
            builder.ToTable("Trainers");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Firstname)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(t => t.Lastname)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(t => t.Bio)
                .HasMaxLength(1000);

            builder.Property(t => t.ProfileImageUrl)
                .HasMaxLength(250);

            builder.Property(t => t.InstagramHandle)
                .HasMaxLength(100);

        }
    }
}
