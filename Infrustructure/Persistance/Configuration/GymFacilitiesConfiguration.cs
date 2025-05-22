using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configuration
{
    public class GymFacilitiesConfiguration : IEntityTypeConfiguration<GymFacility>
    {
        public void Configure(EntityTypeBuilder<GymFacility> builder)
        {
            builder.ToTable("GymFacilities");

            builder.HasKey(f => f.Id);

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

            builder.Property(f => f.CreatedDate)
                   .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
