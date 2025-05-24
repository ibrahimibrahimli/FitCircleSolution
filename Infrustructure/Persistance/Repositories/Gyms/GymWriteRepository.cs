using Application.Abstracts.Repositories.Gyms;
using Domain.Entities;
using Persistance.Context;

namespace Persistance.Repositories.Gyms
{
    public class GymWriteRepository : GenericWriteRepository<Gym>, IGymWriteRepository
    {
        public GymWriteRepository(FitCircleDbContext context) : base(context)
        {
        }
    }
}
