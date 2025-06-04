using Microsoft.AspNetCore.Identity;

namespace Domain.Identity
{
    public class ApplicationRole : IdentityRole<string>
    {
        public string DisplayName { get; private set; } = string.Empty;

        public string? Description { get; private set; }

        public bool IsSystemRole { get; private set; }

        public bool IsActive { get; private set; } = true;

        public DateTime CreatedAt { get; private set; }

        public DateTime? ModifiedAt { get; private set; }

        public string? CreatedBy { get; private set; }

        public string? ModifiedBy { get; private set; }

        public int Priority { get; private set; } = 0; // Higher number = higher priority

        // Navigation properties
        public ICollection<ApplicationUserRole> UserRoles { get; private set; } = new List<ApplicationUserRole>();
        public ICollection<ApplicationRoleClaim> RoleClaims { get; private set; } = new List<ApplicationRoleClaim>();

        // Private constructor for EF Core
        private ApplicationRole()
        {
            CreatedAt = DateTime.UtcNow;
        }

        public ApplicationRole(string name, string displayName, string? description = null, bool isSystemRole = false, int priority = 0)
        {
            ValidateRoleName(name);
            ValidateDisplayName(displayName);

            Id = Guid.NewGuid().ToString();
            Name = name.ToUpperInvariant();
            NormalizedName = name.ToUpperInvariant();
            DisplayName = displayName.Trim();
            Description = description?.Trim();
            IsSystemRole = isSystemRole;
            Priority = priority;
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
        }

        public void UpdateDisplayName(string displayName, string? modifiedBy = null)
        {
            ValidateDisplayName(displayName);

            DisplayName = displayName.Trim();
            ModifiedAt = DateTime.UtcNow;
            ModifiedBy = modifiedBy;
        }

        public void UpdateDescription(string? description, string? modifiedBy = null)
        {
            Description = description?.Trim();
            ModifiedAt = DateTime.UtcNow;
            ModifiedBy = modifiedBy;
        }

        public void UpdatePriority(int priority, string? modifiedBy = null)
        {
            if (priority < 0)
                throw new ArgumentException("Prioritet mənfi ola bilməz.");

            Priority = priority;
            ModifiedAt = DateTime.UtcNow;
            ModifiedBy = modifiedBy;
        }

        public void Deactivate(string? modifiedBy = null)
        {
            if (IsSystemRole)
                throw new InvalidOperationException("Sistem rolları deaktiv edilə bilməz.");

            if (!IsActive)
                throw new InvalidOperationException("Rol artıq deaktivdir.");

            IsActive = false;
            ModifiedAt = DateTime.UtcNow;
            ModifiedBy = modifiedBy;
        }

        public void Activate(string? modifiedBy = null)
        {
            if (IsActive)
                throw new InvalidOperationException("Rol artıq aktivdir.");

            IsActive = true;
            ModifiedAt = DateTime.UtcNow;
            ModifiedBy = modifiedBy;
        }

        public bool CanBeDeleted()
        {
            return !IsSystemRole && IsActive && !UserRoles.Any();
        }

        public bool CanBeModified()
        {
            return !IsSystemRole;
        }

        private static void ValidateRoleName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Rol adı boş ola bilməz.");

            if (name.Trim().Length < 2)
                throw new ArgumentException("Rol adı ən azı 2 simvol olmalıdır.");

            if (name.Contains(" "))
                throw new ArgumentException("Rol adında boşluq ola bilməz.");
        }

        private static void ValidateDisplayName(string displayName)
        {
            if (string.IsNullOrWhiteSpace(displayName))
                throw new ArgumentException("Rol görüntü adı boş ola bilməz.");

            if (displayName.Trim().Length < 2)
                throw new ArgumentException("Rol görüntü adı ən azı 2 simvol olmalıdır.");
        }
    }

    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public DateTime AssignedAt { get; private set; }

        public string? AssignedBy { get; private set; }

        public DateTime? ExpiresAt { get; private set; }

        public bool IsActive => !ExpiresAt.HasValue || ExpiresAt.Value > DateTime.UtcNow;

        public ApplicationUser User { get; private set; } = null!;

        public ApplicationRole Role { get; private set; } = null!;

        private ApplicationUserRole()
        {
            AssignedAt = DateTime.UtcNow;
        }

        public ApplicationUserRole(Guid userId, Guid roleId, string? assignedBy = null, DateTime? expiresAt = null)
        {
            UserId = userId.ToString();
            RoleId = roleId.ToString();
            AssignedBy = assignedBy;
            ExpiresAt = expiresAt;
            AssignedAt = DateTime.UtcNow;
        }

        public void ExtendExpiration(DateTime newExpirationDate, string? modifiedBy = null)
        {
            if (newExpirationDate <= DateTime.UtcNow)
                throw new ArgumentException("Yeni bitmə tarixi indiki vaxtdan sonra olmalıdır.");

            ExpiresAt = newExpirationDate;
            AssignedBy = modifiedBy ?? AssignedBy;
        }

        public void RemoveExpiration(string? modifiedBy = null)
        {
            ExpiresAt = null;
            AssignedBy = modifiedBy ?? AssignedBy;
        }
    }
}