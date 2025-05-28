using Domain.Entities;

namespace Application.Abstracts.Repositories.UserSubcriptions
{
    public interface IUserSubscriptionReadRepository : IReadRepository<UserSubcription>
    {
        Task<UserSubcription?> GetActiveSubscriptionByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
