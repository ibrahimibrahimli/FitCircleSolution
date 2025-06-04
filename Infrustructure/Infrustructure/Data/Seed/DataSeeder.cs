using Domain.Constants;
using Domain.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Infrustructure.Data.Seed
{
    public static class DataSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            SeedSystemRoles(modelBuilder);
            SeedSystemRoleClaims(modelBuilder);
        }

        private static void SeedSystemRoles(ModelBuilder modelBuilder)
        {
            var systemRoles = new List<ApplicationRole>();

            foreach (var roleDef in Roles.RoleDefinitions)
            {
                systemRoles.Add(new ApplicationRole(
                    roleDef.Key,
                    roleDef.Value.DisplayName,
                    roleDef.Value.Description,
                    isSystemRole: true,
                    priority: roleDef.Value.Priority));
            }

            //modelBuilder.Entity<ApplicationRole>().HasData(systemRoles);
        }

        private static void SeedSystemRoleClaims(ModelBuilder modelBuilder)
        {
            var systemClaims = new List<ApplicationRoleClaim>();
            int claimId = 1;

            var allClaims = typeof(Claims).GetFields()
                .Where(f => f.IsStatic && f.IsLiteral)
                .Select(f => f.GetRawConstantValue()?.ToString())
                .Where(val => !string.IsNullOrEmpty(val))
                .ToList();

            // SuperAdmin gets all claims
            var superAdminRoleId = Roles.SuperAdmin;

            foreach (var claim in allClaims)
            {
                systemClaims.Add(new ApplicationRoleClaim
                {
                    Id = claimId++,
                    RoleId = superAdminRoleId,
                    ClaimType = Claims.CanManageSystemSettings,
                    ClaimValue = claim!
                });
            }

            //modelBuilder.Entity<ApplicationRoleClaim>().HasData(systemClaims);
        }
    }
}
