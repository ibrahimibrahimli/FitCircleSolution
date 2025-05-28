using Application.Abstracts.Repositories.UserSubcriptions;
using Domain.Entities;
using Persistance.Context;

namespace Persistance.Repositories.UserSubscriptions
{
    public class UserSubscriptionReadRepository : GenericReadRepository<UserSubcription>, IUserSubscriptionReadRepository
    {
        public UserSubscriptionReadRepository(FitCircleDbContext context) : base(context)
        {
        }

        public Task<UserSubcription?> GetActiveSubscriptionByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
