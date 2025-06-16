using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.Identity;
using Domain.ValueObjects;
using Infrustructure.Data.Seed;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistance.Configuration;
using System.Reflection.Emit;

namespace Persistance.Context
{
    public class FitCircleDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        readonly ICurrentUserService _currentUserService;

        public FitCircleDbContext(DbContextOptions<FitCircleDbContext> options, ICurrentUserService currentUserService) : base(options)
        {
            _currentUserService = currentUserService;
        }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<ApplicationRole> Roles { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<DietPlan> DietPlans { get; set; }
        public DbSet<DietPlanMeal> DietPlanMeals { get; set; }
        public DbSet<DietPlanProgress> DietPlanProgresses { get; set; }
        public DbSet<Gym> Gyms { get; set; }
        public DbSet<GymFacility> GymFacilities { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<TrainerRating> TrainerRatings { get; set; }
        public DbSet<UserSubscription> UserSubscriptions { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<WorkoutExercise> WorkoutExercises { get; set; }
        public DbSet<WorkoutPlan> WorkoutPlans { get; set; }
        public DbSet<WorkoutPlanProgress> WorkoutPlanProgresses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Ignore<GymFacilityType>();


            // Configuration-ları tətbiq et
            builder.ApplyConfigurationsFromAssembly(typeof(FitCircleDbContext).Assembly);

            // Global query filter-lər (məsələn soft delete üçün)
            ConfigureGlobalFilters(builder);

            DataSeeder.Seed(builder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Audit informasiyasını yenilə
            UpdateAuditInformation();

            return await base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            // Audit informasiyasını yenilə
            UpdateAuditInformation();

            return base.SaveChanges();
        }

        private void UpdateAuditInformation()
        {
            var userId = _currentUserService.GetCurrentUserId;

            var auditableEntities = ChangeTracker
                .Entries<BaseAuditableEntity>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            var utcNow = DateTime.UtcNow;

            foreach (var entityEntry in auditableEntities)
            {
                switch (entityEntry.State)
                {
                    case EntityState.Added:
                        entityEntry.Entity.CreatedAt = utcNow;
                        entityEntry.Entity.CreatedBy = userId ?? "System Admin";

                        if (entityEntry.Entity.GetType().GetProperty("UpdatedDate") != null)
                        {
                            entityEntry.Property("UpdatedDate").CurrentValue = utcNow;
                        }
                        break;

                    case EntityState.Modified:
                        entityEntry.Property(e => e.CreatedAt).IsModified = false;
                        entityEntry.Entity.UpdatedBy = userId ?? "System Admin" ;
                        if (entityEntry.Entity.GetType().GetProperty("UpdatedDate") != null)
                        {
                            entityEntry.Property("UpdatedDate").CurrentValue = utcNow;
                        }
                        break;
                }
            }
        }

        private void ConfigureGlobalFilters(ModelBuilder builder)
        {
            // Soft delete filter (əgər ISoftDelete interface-i varsa)
            // builder.Entity<BaseAuditableEntity>()
            //     .HasQueryFilter(e => !e.IsDeleted);

            // Digər global filter-lər burada əlavə edilə bilər
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            // Development zamanı sensitive data logging-i aktiv et
#if DEBUG
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
#endif
        }
    }
}