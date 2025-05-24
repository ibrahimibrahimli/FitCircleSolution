using Application.Abstracts.Repositories.TrainerRatings;
using Domain.Entities;
using Persistance.Context;

namespace Persistance.Repositories.TrainerRatings
{
    public class TrainerRatingReadRepository : GenericReadRepository<TrainerRating>, ITrainerRatingReadRepository
    {
        public TrainerRatingReadRepository(FitCircleDbContext context) : base(context)
        {
        }
    }
}
