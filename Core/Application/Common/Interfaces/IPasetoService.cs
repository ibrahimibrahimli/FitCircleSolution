using System.Security.Claims;

namespace Application.Common.Interfaces;

public interface IPasetoService
{
    public ClaimsPrincipal ValidateToken(string token);
    public string GenerateToken(string userCode, string name, string fullName, string userRole);
}
