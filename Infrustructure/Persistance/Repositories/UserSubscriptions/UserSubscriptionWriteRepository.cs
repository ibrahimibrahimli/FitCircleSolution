using Application.Abstracts.Repositories.UserSubcriptions;
using Domain.Entities;
using Persistance.Context;

namespace Persistance.Repositories.UserSubscriptions
{
    public class UserSubscriptionWriteRepository : GenericWriteRepository<UserSubscription>, IUserSubscriptionWriteRepository
    {
        public UserSubscriptionWriteRepository(FitCircleDbContext context) : base(context)
        {
        }
    }
}
