//-----------------------------------------------------------------------
// <copyright file="VersionService.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
// Ignore Spelling: Blazor

using ROH.Site.Interfaces.Api;
using ROH.Site.Interfaces.Helpers;
using ROH.StandardModels.Response;
using ROH.StandardModels.Version;
using ROH.Utils.ApiConfiguration;

namespace ROH.Site.Api;

public class VersionService(ICustomAuthenticationStateProvider customAuthenticationStateProvider) : IVersionService
{
    readonly Gateway _gateway = new();

    public async Task<DefaultResponse?> CreateNewVersion(GameVersionModel model) => await _gateway.PostAsync(
        Gateway.Services.CreateNewVersion,
        model,
        await customAuthenticationStateProvider.GetToken());

    public async Task<DefaultResponse?> GetAllReleasedVersionsPaginated(int page = 1, int take = 10) => await _gateway.GetAsync(
        Gateway.Services.GetAllReleasedVersionsPaginated,
        new { Page = page, Take = take },
        await customAuthenticationStateProvider.GetToken());

    public async Task<DefaultResponse?> GetAllVersionsPaginated(int page = 1, int take = 10) => await _gateway.GetAsync(
        Gateway.Services.GetAllVersionsPaginated,
        new { Page = page, Take = take },
        await customAuthenticationStateProvider.GetToken());

    public async Task<DefaultResponse?> GetCurrentVersion() => await _gateway.GetAsync<object?>(
        Gateway.Services.GetCurrentVersion,
        null,
        await customAuthenticationStateProvider.GetToken());

    public async Task<DefaultResponse?> GetVersionDetails(Guid guid) => await _gateway.GetAsync(
        Gateway.Services.GetVersionDetails,
        new { Guid = guid },
        await customAuthenticationStateProvider.GetToken());

    public async Task<DefaultResponse?> ReleaseVersion(GameVersionModel model) => await _gateway.UpdateAsync(
        Gateway.Services.ReleaseVersion,
        model,
        await customAuthenticationStateProvider.GetToken());
}