using Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configuration
{
    public class ApplicationRoleClaimConfiguration : IEntityTypeConfiguration<ApplicationRoleClaim>
    {
        public void Configure(EntityTypeBuilder<ApplicationRoleClaim> builder)
        {
            builder.HasOne(rc => rc.Role)
            .WithMany(r => r.RoleClaims)
            .HasForeignKey(rc => rc.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
