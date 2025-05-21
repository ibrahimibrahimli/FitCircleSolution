using Application.Abstracts.Repositories.GymFacilities;
using Domain.Entities;
using Persistance.Context;

namespace Persistance.Repositories.GymFacilities
{
    public class GymFacilityWriteRepository : GenericWriteRepository<GymFacility>, IGymFacilityWriteRepository
    {
        public GymFacilityWriteRepository(FitCircleDbContext context) : base(context)
        {
        }
    }
}
