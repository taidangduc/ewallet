using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace EWallet.Common.Web;

public interface ICurrentWebUser
{
    Guid UserId { get; }
    bool IsAuthenticated { get; }
}

public class CurrentWebUser : ICurrentWebUser
{
    private readonly IHttpContextAccessor _context;

    public CurrentWebUser(IHttpContextAccessor context)
    {
        _context = context;
    }

    public Guid UserId
    {
        get
        {
            var userId = _context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? _context.HttpContext.User.FindFirst("sub")?.Value;
            return string.IsNullOrEmpty(userId) ? Guid.Empty : Guid.Parse(userId);
        }
    }

    public bool IsAuthenticated
    {
        get
        {
            return _context.HttpContext.User.Identity.IsAuthenticated;
        }
    }
}