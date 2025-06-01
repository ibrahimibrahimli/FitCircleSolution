using Application.Abstracts.Repositories.UserSubcriptions;
using Domain.Entities;
using Persistance.Context;

namespace Persistance.Repositories.UserSubscriptions
{
    public class UserSubscriptionReadRepository : GenericReadRepository<UserSubscription>, IUserSubscriptionReadRepository
    {
        public UserSubscriptionReadRepository(FitCircleDbContext context) : base(context)
        {
        }

        public Task<UserSubscription?> GetActiveSubscriptionByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
