using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Persistance.Configuration
{
    public class GymFacilityTypeConfiguration : IEntityTypeConfiguration<GymFacilityType>
    {
        public void Configure(EntityTypeBuilder<GymFacilityType> builder)
        {
            builder.HasNoKey();
        }
    }
}
