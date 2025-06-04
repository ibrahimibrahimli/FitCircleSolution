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
}
