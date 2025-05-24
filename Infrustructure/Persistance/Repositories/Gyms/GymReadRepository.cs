using Application.Abstracts.Repositories.Gyms;
using Domain.Entities;
using Persistance.Context;

namespace Persistance.Repositories.Gyms
{
    public class GymReadRepository : GenericReadRepository<Gym>, IGymReadRepository
    {
        public GymReadRepository(FitCircleDbContext context) : base(context)
        {
        }
    }
}
