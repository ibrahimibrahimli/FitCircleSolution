using Application.Common.Interfaces;
using Application.Common.Mapping;
using Application.Common.Services.AccessControl;
using Application.Features.Cities.Commands.Create;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(GymProfile).Assembly);

            //services.AddScoped<IAccessControlService, AccessControlService>();
            return services;
        }
    }
}
