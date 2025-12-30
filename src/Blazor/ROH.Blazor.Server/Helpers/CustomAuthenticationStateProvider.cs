//-----------------------------------------------------------------------
// <copyright file="CustomAuthenticationStateProvider.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
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
    private bool _isInitialized;
    private readonly string _userKey = "userKey";

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (!_isInitialized)
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        string? token = await localStorage.GetItemAsync<string>("_tokenKey");

        token = token?.Trim('"');

        if (string.IsNullOrEmpty(token))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        IEnumerable<Claim> claims = JwtParser.ParseClaimsFromJwt(token);

        ClaimsPrincipal user = new(new ClaimsIdentity(claims, "jwtAuthType"));

        return new AuthenticationState(user);
    }

    public async Task<string> GetToken()
    {
        if (!_isInitialized)
        {
            return string.Empty;
        }

        string? token = await localStorage.GetItemAsStringAsync(_authToken);
        return token?.Trim('"') ?? string.Empty;
    }

    public async Task<string> GetUser() => _isInitialized
        ? (await localStorage.GetItemAsStringAsync(_userKey) ?? string.Empty)
        : string.Empty;

    public void Initialize()
    {
        _isInitialized = true;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task MarkUserAsAuthenticated(string token)
    {
        IEnumerable<Claim> claims = JwtParser.ParseClaimsFromJwt(token);

        await localStorage.SetItemAsync("_tokenKey", token);

        ClaimsPrincipal authenticatedUser = new(new ClaimsIdentity(claims, "jwtAuthType"));

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(authenticatedUser)));
    }

    public async Task MarkUserAsLoggedOut()
    {
        await localStorage.RemoveItemAsync("authToken").ConfigureAwait(false);

        ClaimsPrincipal anonymousUser = new(new ClaimsIdentity());

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymousUser)));
    }

    public async Task SetUserToken(string user = "", string token = "")
    {
        await localStorage.SetItemAsync(_authToken, token);
        await localStorage.SetItemAsync(_userKey, user);
    }
}