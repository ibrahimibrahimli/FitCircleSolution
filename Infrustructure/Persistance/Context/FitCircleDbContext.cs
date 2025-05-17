using Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Context
{
    public class FitCircleDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public FitCircleDbContext(DbContextOptions<FitCircleDbContext> options) : base(options) { }
    }
}
