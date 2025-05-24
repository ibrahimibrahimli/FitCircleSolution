using Domain.Common;
using Domain.Entities;
using Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistance.Configuration;
using System.Reflection.Emit;

namespace Persistance.Context
{
    public class FitCircleDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public FitCircleDbContext(DbContextOptions<FitCircleDbContext> options) : base(options) { }

        public DbSet<City> Citys { get; set; }
        public DbSet<Gym> Gyms { get; set; }
        public DbSet<GymFacility> GymFacilities { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<TrainerRating> TrainerRatings { get; set; }
        public DbSet<Country> Countries { get; set; }





        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(FitCircleDbContext).Assembly);

        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var data = ChangeTracker.Entries<BaseAuditableEntity>();
            foreach (var item in data)
            {
                _ = item.State switch
                {
                    EntityState.Added => item.Entity.CreatedDate = DateTime.UtcNow,
                    EntityState.Modified => item.Entity.UpdatedDate = DateTime.UtcNow,
                    _ => DateTime.UtcNow
                };
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
