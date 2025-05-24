using Application.Abstracts.Repositories.Trainers;
using Domain.Entities;
using Persistance.Context;

namespace Persistance.Repositories.Trainers
{
    public class TrainerReadRepository : GenericReadRepository<Trainer>, ITrainerReadRepository
    {
        public TrainerReadRepository(FitCircleDbContext context) : base(context)
        {
        }
    }
}
