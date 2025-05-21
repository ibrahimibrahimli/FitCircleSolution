using Application.Abstracts.Repositories.GymTypes;
using Domain.Entities;
using Persistance.Context;

namespace Persistance.Repositories.GymTypes
{
    public class GymTypeReadRepository : GenericReadRepository<GymType>, IGymTypeReadRepository
    {
        public GymTypeReadRepository(FitCircleDbContext context) : base(context)
        {
        }
    }
}
