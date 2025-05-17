using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistance.Context;

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

            return services;

        }
    }
}
