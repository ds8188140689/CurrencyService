using CurrencyService.Shared.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CurrencyService.Shared.Infrastructure.Auth;

///<inheritdoc/>
public class CurrentUserAccessor(IHttpContextAccessor httpContextAccessor) : ICurrentUserAccessor
{
    ///<inheritdoc/>
    public Guid? UserId => Guid.TryParse(httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId) ? userId : null;

    ///<inheritdoc/>
    public string? UserName => httpContextAccessor.HttpContext?.User.Identity?.Name;

    ///<inheritdoc/>
    public bool IsAuthenticated => httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true;
}
