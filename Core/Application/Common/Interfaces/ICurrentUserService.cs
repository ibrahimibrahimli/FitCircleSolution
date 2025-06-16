namespace Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string? GetCurrentUserId();
        string GetCurrentUsername();
        string GetCurrentUserRole();
        string GetCurrentUserFullName();
        string GetUserAgent();
        string GetFingerprint();
        string GetRemoteIpAddress();
        string GetRefreshToken();
        string GetAccessToken();
    }
}
