using System.Security.Claims;
using Console.Contracts;

namespace Console.Infrastructure;

public class UserContextService:IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public ClaimsPrincipal GetUser()
    {
        return _httpContextAccessor.HttpContext?.User as ClaimsPrincipal;
    }

    public int GetUserId()
    {
        var value = GetUser().FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        if (value != null)
            return int.Parse(value);
        throw new NullReferenceException();
    }
}