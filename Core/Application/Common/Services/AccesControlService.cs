using Application.Abstracts.Repositories.Gyms;
using Application.Abstracts.Repositories.UserSubcriptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces;
using Application.Common.Interfaces;
using Domain.Enums;

namespace Application.Common.Services.AccessControl;

public class AccessControlService : IAccessControlService
{
    private readonly IUserSubscriptionReadRepository _subscriptionReadRepository;
    private readonly IGymReadRepository _gymReadRepository;

    public AccessControlService(
        IUserSubscriptionReadRepository subscriptionRepository,
        IGymReadRepository gymRepository)
    {
        _subscriptionReadRepository = subscriptionRepository;
        _gymReadRepository = gymRepository;
    }

    public async Task<bool> CanUserAccessGymAsync(Guid userId, Guid gymId, CancellationToken cancellationToken = default)
    {
        var subscription = await _subscriptionReadRepository.GetActiveSubscriptionByUserIdAsync(userId, cancellationToken);
        if (subscription is null)
            return false;

        var gym = await _gymReadRepository.GetByIdAsync(gymId, cancellationToken);
        if (gym is null)
            return false;

       
        return subscription.Type switch
        {
            SubscriptionType.Vip => true,
            SubscriptionType.Premium => gym.MonthlyPrice <= 100,
            SubscriptionType.Basic => gym.MonthlyPrice <= 60,
            _ => false
        };
    }
}
