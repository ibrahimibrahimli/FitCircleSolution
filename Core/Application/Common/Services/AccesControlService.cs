using Application.Abstracts.Repositories.Gyms;
using Application.Common.Interfaces;
using Application.Common.Interfaces.AccessControl;
using Application.Common.Interfaces.Persistence;
using Domain.Enums;

namespace Application.Common.Services.AccessControl;

public class AccessControlService : IAccessControlService
{
    private readonly IUserSubscriptionRepository _subscriptionRepository;
    private readonly IGymReadRepository _gymReadRepository;

    public AccessControlService(
        IUserSubscriptionRepository subscriptionRepository,
        IGymReadRepository gymRepository)
    {
        _subscriptionRepository = subscriptionRepository;
        _gymReadRepository = gymRepository;
    }

    public async Task<bool> CanUserAccessGymAsync(Guid userId, Guid gymId, CancellationToken cancellationToken = default)
    {
        var subscription = await _subscriptionRepository.GetActiveSubscriptionByUserIdAsync(userId, cancellationToken);
        if (subscription is null)
            return false;

        var gym = await _gymReadRepository.GetByIdAsync(gymId, cancellationToken);
        if (gym is null)
            return false;

       
        return subscription.Type switch
        {
            SubcriptionType.Vip => true,
            SubcriptionType.Premium => gym.MonthlyPrice <= 100,
            SubcriptionType.Gold => gym.MonthlyPrice <= 80,
            SubcriptionType.Standart => gym.MonthlyPrice <= 60,
            _ => false
        };
    }
}
