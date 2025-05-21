using Application.Abstracts.Repositories.Cities;
using Domain.Entities;
using Persistance.Context;

namespace Persistance.Repositories.Cities
{
    public class CityWriteRepository : GenericWriteRepository<City>, ICityWriteRepository
    {
        public CityWriteRepository(FitCircleDbContext context) : base(context)
        {
        }
    }
}
