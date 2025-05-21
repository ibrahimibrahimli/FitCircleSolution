using Application.Abstracts.Repositories.TrainerRatings;
using Domain.Entities;
using Persistance.Context;

namespace Persistance.Repositories.TrainerRatings
{
    public class TrainerRatingWriteRepository : GenericWriteRepository<TrainerRating>, ITrainerRatingWriteRepository
    {
        public TrainerRatingWriteRepository(FitCircleDbContext context) : base(context)
        {
        }
    }
}
