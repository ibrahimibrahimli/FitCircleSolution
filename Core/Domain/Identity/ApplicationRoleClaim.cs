using Domain.Constants;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Domain.Identity
{
    public class ApplicationRoleClaim : IdentityRoleClaim<string>
    {
        public DateTime CreatedAt { get; private set; }

        public string? CreatedBy { get; private set; }

        public ApplicationRole Role { get; private set; } = null!;

        public ApplicationRoleClaim()
        {
            CreatedAt = DateTime.UtcNow;
        }

        public ApplicationRoleClaim(string roleId, string claimType, string claimValue, string? createdBy = null)
        {
            RoleId = roleId;
            ClaimType = claimType;
            ClaimValue = claimValue;
            CreatedBy = createdBy;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
