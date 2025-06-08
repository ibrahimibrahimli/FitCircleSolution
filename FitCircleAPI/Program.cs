
using Application.Extensions;
using Application.Features.Cities.Commands.Create;
using FitCircleAPI.Exsensions;
using Infrustructure.Extensions;
using Persistance.Extensions;

namespace FitCircleAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.


            builder.Services.AddPersistance(builder.Configuration);
            builder.Services.AddInfrustructure();
            builder.Services.AddApplication();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(CreateCityCommandHandler).Assembly);
            });

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapSwagger();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.UseApplicationMiddlewares();

            app.Run();
        }
    }
}
