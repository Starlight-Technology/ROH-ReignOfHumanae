using Microsoft.AspNetCore.Components.Authorization;

using System.Security.Claims;

namespace ROH.Blazor.Server.Helpers;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomAuthenticationStateProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        return Task.FromResult(new AuthenticationState(user ?? new ClaimsPrincipal(new ClaimsIdentity())));
    }
}
