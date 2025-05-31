using Microsoft.AspNetCore.Identity;

namespace Domain.Identity
{
    public class ApplicationRole : IdentityRole<Guid>
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

            Id = Guid.NewGuid();
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

    public class ApplicationUserRole : IdentityUserRole<Guid>
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
            UserId = userId;
            RoleId = roleId;
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

    public class ApplicationRoleClaim : IdentityRoleClaim<Guid>
    {
        public DateTime CreatedAt { get; private set; }

        public string? CreatedBy { get; private set; }

        public ApplicationRole Role { get; private set; } = null!;

        private ApplicationRoleClaim()
        {
            CreatedAt = DateTime.UtcNow;
        }

        public ApplicationRoleClaim(Guid roleId, string claimType, string claimValue, string? createdBy = null)
        {
            RoleId = roleId;
            ClaimType = claimType;
            ClaimValue = claimValue;
            CreatedBy = createdBy;
            CreatedAt = DateTime.UtcNow;
        }
    }
}

namespace Domain.Constants
{
    public static class Roles
    {
        public const string SuperAdmin = "SUPERADMIN";
        public const string Admin = "ADMIN";
        public const string Manager = "MANAGER";
        public const string Trainer = "TRAINER";
        public const string Member = "MEMBER";
        public const string Guest = "GUEST";

        public static readonly Dictionary<string, (string DisplayName, string Description, int Priority)> RoleDefinitions = new()
        {
            {
                SuperAdmin,
                ("Super Admin", "Sistemin bütün funksiyalarına tam giriş", 100)
            },
            {
                Admin,
                ("Administrator", "Sistem idarəetməsi və istifadəçi idarəetməsi", 90)
            },
            {
                Manager,
                ("Menecer", "Məşqçi və üzv idarəetməsi", 80)
            },
            {
                Trainer,
                ("Məşqçi", "Antrenman planları və diyeta həkim məsləhəti", 70)
            },
            {
                Member,
                ("Üzv", "Standart üzv icazələri", 60)
            },
            {
                Guest,
                ("Qonaq", "Məhdud giriş icazələri", 50)
            }
        };

        public static List<string> GetAllRoles() => RoleDefinitions.Keys.ToList();

        public static bool IsSystemRole(string roleName) => RoleDefinitions.ContainsKey(roleName.ToUpperInvariant());

        public static string GetDisplayName(string roleName)
        {
            return RoleDefinitions.TryGetValue(roleName.ToUpperInvariant(), out var definition)
                ? definition.DisplayName
                : roleName;
        }

        public static string GetDescription(string roleName)
        {
            return RoleDefinitions.TryGetValue(roleName.ToUpperInvariant(), out var definition)
                ? definition.Description
                : string.Empty;
        }

        public static int GetPriority(string roleName)
        {
            return RoleDefinitions.TryGetValue(roleName.ToUpperInvariant(), out var definition)
                ? definition.Priority
                : 0;
        }
    }

    public static class Claims
    {
        // User Management
        public const string CanViewUsers = "users.view";
        public const string CanCreateUsers = "users.create";
        public const string CanEditUsers = "users.edit";
        public const string CanDeleteUsers = "users.delete";

        // Role Management
        public const string CanViewRoles = "roles.view";
        public const string CanCreateRoles = "roles.create";
        public const string CanEditRoles = "roles.edit";
        public const string CanDeleteRoles = "roles.delete";
        public const string CanAssignRoles = "roles.assign";

        // Subscription Management
        public const string CanViewSubscriptions = "subscriptions.view";
        public const string CanCreateSubscriptions = "subscriptions.create";
        public const string CanEditSubscriptions = "subscriptions.edit";
        public const string CanCancelSubscriptions = "subscriptions.cancel";

        // Payment Management
        public const string CanViewPayments = "payments.view";
        public const string CanProcessPayments = "payments.process";
        public const string CanRefundPayments = "payments.refund";

        // Trainer Management
        public const string CanViewTrainers = "trainers.view";
        public const string CanCreateTrainers = "trainers.create";
        public const string CanEditTrainers = "trainers.edit";
        public const string CanDeleteTrainers = "trainers.delete";
        public const string CanAssignTrainers = "trainers.assign";

        // Workout Plan Management
        public const string CanViewWorkoutPlans = "workoutplans.view";
        public const string CanCreateWorkoutPlans = "workoutplans.create";
        public const string CanEditWorkoutPlans = "workoutplans.edit";
        public const string CanDeleteWorkoutPlans = "workoutplans.delete";

        // Diet Plan Management
        public const string CanViewDietPlans = "dietplans.view";
        public const string CanCreateDietPlans = "dietplans.create";
        public const string CanEditDietPlans = "dietplans.edit";
        public const string CanDeleteDietPlans = "dietplans.delete";

        // Reports
        public const string CanViewReports = "reports.view";
        public const string CanExportReports = "reports.export";

        // System
        public const string CanViewSystemLogs = "system.logs.view";
        public const string CanManageSystemSettings = "system.settings.manage";
    }
}