using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Application.Common.Interfaces;

namespace Persistance.Context
{
    public class FitCircleDbContextFactory : IDesignTimeDbContextFactory<FitCircleDbContext>
    {
        readonly ICurrentUserService _currentUserService;

       

        public FitCircleDbContext CreateDbContext(string[] args)
        {
            // Layihənin kök qovluğundakı appsettings.json-u oxuyur
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
                
            var connectionString = configuration.GetConnectionString("SqlServer");

            var optionsBuilder = new DbContextOptionsBuilder<FitCircleDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new FitCircleDbContext(optionsBuilder.Options, _currentUserService );
        }
    }
}
