using Application.Abstracts.Repositories.GymFacilities;
using Domain.Entities;
using Persistance.Context;

namespace Persistance.Repositories.GymFacilities
{
    public class GymFacilityReadRepository : GenericReadRepository<GymFacility>, IGymFacilityReadRepository
    {
        public GymFacilityReadRepository(FitCircleDbContext context) : base(context)
        {
        }
    }
}
