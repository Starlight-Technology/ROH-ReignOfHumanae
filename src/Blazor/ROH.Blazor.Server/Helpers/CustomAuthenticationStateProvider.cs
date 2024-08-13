using Microsoft.AspNetCore.Components.Authorization;

using System.Security.Claims;

namespace ROH.Blazor.Server.Helpers;

public class CustomAuthenticationStateProvider(IHttpContextAccessor httpContextAccessor) : AuthenticationStateProvider
{
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var user = httpContextAccessor.HttpContext?.User;

        return Task.FromResult(new AuthenticationState(user ?? new ClaimsPrincipal(new ClaimsIdentity())));
    }
}
