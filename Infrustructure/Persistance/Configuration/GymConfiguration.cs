using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configuration
{
    public class GymConfiguration : IEntityTypeConfiguration<Gym>
    {
        public void Configure(EntityTypeBuilder<Gym> builder)
        {
            builder.ToTable("Gyms");

            builder.HasKey(g => g.Id);

            builder.Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(g => g.Address)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(g => g.City)
                .HasMaxLength(100);

            builder.Property(g => g.Country)
                .HasMaxLength(100);

            builder.Property(g => g.PhoneNumber)
                .HasMaxLength(50);

            builder.HasMany(g => g.Facilities)
                .WithOne(f => f.Gym)
                .HasForeignKey(f => f.GymId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(g => g.Trainers)
                .WithOne(t => t.Gym)
                .HasForeignKey(t => t.GymId);
        }
    }
}
