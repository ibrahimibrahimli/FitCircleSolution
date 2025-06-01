using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configuration
{
    public class GymFacilitiesConfiguration : IEntityTypeConfiguration<GymFacility>
    {
        public void Configure(EntityTypeBuilder<GymFacility> builder)
        {
            builder.ToTable("GymFacilities");

            builder.HasKey(f => f.Id);

            builder.Ignore(g => g.FacilityType);

            builder.Property(x => x.FacilityType)
            .HasConversion(
                v => v.Id,  // Database-ə yazarkən
                v => GymFacilityType.FromId(v), // Database-dən oxuyarkən
                new ValueComparer<GymFacilityType>(
                    (c1, c2) => c1.Id == c2.Id,
                    c => c.Id.GetHashCode(),
                    c => GymFacilityType.FromId(c.Id)))
            .HasColumnName("FacilityTypeId");

            builder.Property(f => f.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(f => f.Description)
                   .HasMaxLength(500);

            builder.Property(f => f.GymId)
                   .IsRequired();

            builder.HasOne(f => f.Gym)
                   .WithMany(g => g.Facilities)
                   .HasForeignKey(f => f.GymId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(f => f.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
