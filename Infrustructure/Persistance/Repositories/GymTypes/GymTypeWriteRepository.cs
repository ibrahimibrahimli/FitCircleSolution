using Application.Abstracts.Repositories.GymTypes;
using Domain.Entities;
using Persistance.Context;

namespace Persistance.Repositories.GymTypes
{
    public class GymTypeWriteRepository : GenericWriteRepository<GymType>, IGymTypeWriteRepository
    {
        public GymTypeWriteRepository(FitCircleDbContext context) : base(context)
        {
        }
    }
}
