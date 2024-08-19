using Blazored.LocalStorage;

using Microsoft.AspNetCore.Components.Authorization;

using ROH.Blazor.Server.Interfaces.Helpers;

using System.Security.Claims;

namespace ROH.Blazor.Server.Helpers;

/// <summary>
/// I tried to use AuthenticationState with claims identity, but don't worked so i make just with jwt and local storage
/// </summary>
public class CustomAuthenticationStateProvider(ILocalStorageService localStorage) : AuthenticationStateProvider, ICustomAuthenticationStateProvider
{
    private readonly string _authToken = "authToken";
    private readonly string _userKey = "userKey";
    private bool _isInitialized;

    public async Task<string> GetToken() => _isInitialized ? await localStorage.GetItemAsStringAsync(_authToken) ?? "" : "";

    public async Task<string> GetUser() => _isInitialized ? await localStorage.GetItemAsStringAsync(_userKey) ?? "" : "";

    public async Task SetUserToken(string user = "", string token = "")
    {
        await localStorage.SetItemAsync(_authToken, token);
        await localStorage.SetItemAsync(_userKey, user);
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (!_isInitialized)
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        var token = await localStorage.GetItemAsync<string>("_tokenKey");

        if (string.IsNullOrEmpty(token))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        var claims = JwtParser.ParseClaimsFromJwt(token);

        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwtAuthType"));

        return new AuthenticationState(user);
    }

    public async Task MarkUserAsAuthenticated(string token)
    {
        var claims = JwtParser.ParseClaimsFromJwt(token);

        await localStorage.SetItemAsync("_tokenKey", token);

        var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwtAuthType"));

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(authenticatedUser)));

    }

    public async Task MarkUserAsLoggedOut()
    {
        await localStorage.RemoveItemAsync("authToken");

        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymousUser)));

    }

    public void Initialize()
    {
        _isInitialized = true;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}

