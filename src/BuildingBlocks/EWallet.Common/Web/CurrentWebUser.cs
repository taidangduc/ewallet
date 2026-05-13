using Microsoft.AspNetCore.Http;
namespace EWallet.Common.Web;

public interface ICurrentWebUser
{
    Guid UserId { get; }
    bool IsAuthenticated { get; }
}

public class CurrentWebUser : ICurrentWebUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentWebUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId
    {
        get
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;
            return userId != null ? Guid.Parse(userId) : Guid.Empty;
        }
    }

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
}