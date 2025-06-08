using Application.Common.Interfaces;
using Infrustructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrustructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrustructure(this IServiceCollection services)
        {
            //services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            return services;
        }
    }
}
