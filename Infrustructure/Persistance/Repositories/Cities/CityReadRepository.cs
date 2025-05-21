using Application.Abstracts.Repositories;
using Application.Abstracts.Repositories.Cities;
using Domain.Entities;
using Persistance.Context;

namespace Persistance.Repositories.Cities
{
    public class CityReadRepository : GenericReadRepository<City>, ICityReadRepository
    {
        public CityReadRepository(FitCircleDbContext context) : base(context)
        {
        }
    }
}
