using Domain.Entities;

namespace Application.Abstracts.Repositories.UserSubcriptions
{
    public interface IUserSubscriptionReadRepository : IReadRepository<UserSubscription>
    {
        Task<UserSubscription?> GetActiveSubscriptionByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
