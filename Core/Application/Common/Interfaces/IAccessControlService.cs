namespace Application.Common.Interfaces
{
    public interface IAccessControlService
    {
        Task<bool> CanUserAccessGymAsync(Guid userId, Guid gymId, CancellationToken cancellationToken = default);
    }
}
