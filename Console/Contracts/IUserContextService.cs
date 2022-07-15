using System.Security.Claims;

namespace Console.Contracts;

public interface IUserContextService
{
    public ClaimsPrincipal GetUser();
    public int GetUserId();
}