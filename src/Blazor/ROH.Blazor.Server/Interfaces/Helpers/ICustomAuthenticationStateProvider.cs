//-----------------------------------------------------------------------
// <copyright file="ICustomAuthenticationStateProvider.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.AspNetCore.Components.Authorization;

namespace ROH.Blazor.Server.Interfaces.Helpers;

public interface ICustomAuthenticationStateProvider
{
    Task<AuthenticationState> GetAuthenticationStateAsync();

    Task<string> GetToken();

    Task<string> GetUser();

    void Initialize();

    Task MarkUserAsAuthenticated(string token);

    Task MarkUserAsLoggedOut();

    Task SetUserToken(string user = "", string token = "");
}
