using Microsoft.AspNetCore.Components.Authorization;

namespace ROH.Blazor.Server.Interfaces.Helpers;

public interface ICustomAuthenticationStateProvider
{
    Task<string> GetToken();
    Task<string> GetUser();
    Task SetUserToken(string user = "", string token = "");
    Task<AuthenticationState> GetAuthenticationStateAsync();
    Task MarkUserAsAuthenticated(string token);
    Task MarkUserAsLoggedOut();
    void Initialize();

}
