using Application.Abstracts.Repositories;
using Application.Abstracts.Repositories.Cities;
using Application.Abstracts.Repositories.GymFacilities;
using Application.Abstracts.Repositories.Gyms;
using Application.Abstracts.Repositories.TrainerRatings;
using Application.Abstracts.Repositories.Trainers;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistance.Context;
using Persistance.Repositories;
using Persistance.Repositories.Cities;
using Persistance.Repositories.GymFacilities;
using Persistance.Repositories.Gyms;
using Persistance.Repositories.TrainerRatings;
using Persistance.Repositories.Trainers;

namespace Persistance.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {

            }).AddEntityFrameworkStores<FitCircleDbContext>()
                .AddDefaultTokenProviders();
            services.AddDbContext<FitCircleDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlServer")));

            services.AddScoped(typeof(IReadRepository<>), typeof(GenericReadRepository<>));
            services.AddScoped(typeof(IWriteRepository<>), typeof(GenericWriteRepository<>));

            services.AddScoped<IGymReadRepository, GymReadRepository>();
            services.AddScoped<IGymWriteRepository, GymWriteRepository>();
            services.AddScoped<ICityReadRepository, CityReadRepository>();
            services.AddScoped<ICityWriteRepository, CityWriteRepository>();
            services.AddScoped<IGymFacilityReadRepository, GymFacilityReadRepository>();
            services.AddScoped<IGymFacilityWriteRepository, GymFacilityWriteRepository>();
            services.AddScoped<ITrainerRatingReadRepository, TrainerRatingReadRepository>();
            services.AddScoped<ITrainerRatingWriteRepository, TrainerRatingWriteRepository>();
            services.AddScoped<ITrainerReadRepository, TrainerReadRepository>();
            services.AddScoped<ITrainerWriteRepository, TrainerWriteRepository>();

            return services;

        }
    }
}
