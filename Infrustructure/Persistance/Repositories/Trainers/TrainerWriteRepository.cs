using Application.Abstracts.Repositories.Trainers;
using Domain.Entities;
using Persistance.Context;

namespace Persistance.Repositories.Trainers
{
    public class TrainerWriteRepository : GenericWriteRepository<Trainer>, ITrainerWriteRepository
    {
        public TrainerWriteRepository(FitCircleDbContext context) : base(context)
        {
        }
    }
}
