﻿// Ignore Spelling: Blazor

using ROH.Blazor.Server.Interfaces.Api;
using ROH.Blazor.Server.Interfaces.Helpers;
using ROH.StandardModels.Response;
using ROH.StandardModels.Version;

namespace ROH.Blazor.Server.Api;

public class VersionService(ICustomAuthenticationStateProvider customAuthenticationStateProvider) : IVersionService
{
    private readonly Utils.ApiConfiguration.Gateway _gateway = new();

    public async Task<DefaultResponse?> GetCurrentVersion() => await _gateway.Get<object?>(Utils.ApiConfiguration.Gateway.Services.GetCurrentVersion, null, await customAuthenticationStateProvider.GetToken());

    public async Task<DefaultResponse?> CreateNewVersion(GameVersionModel model) => await _gateway.Post(Utils.ApiConfiguration.Gateway.Services.CreateNewVersion, model, await customAuthenticationStateProvider.GetToken());

    public async Task<DefaultResponse?> GetAllVersionsPaginated(int page = 1, int take = 10) => await _gateway.Get(Utils.ApiConfiguration.Gateway.Services.GetAllVersionsPaginated, new { Page = page, Take = take }, await customAuthenticationStateProvider.GetToken());

    public async Task<DefaultResponse?> GetAllReleasedVersionsPaginated(int page = 1, int take = 10) => await _gateway.Get(Utils.ApiConfiguration.Gateway.Services.GetAllReleasedVersionsPaginated, new { Page = page, Take = take }, await customAuthenticationStateProvider.GetToken());

    public async Task<DefaultResponse?> GetVersionDetails(Guid guid) => await _gateway.Get(Utils.ApiConfiguration.Gateway.Services.GetVersionDetails, new { Guid = guid }, await customAuthenticationStateProvider.GetToken());

    public async Task<DefaultResponse?> ReleaseVersion(GameVersionModel model) => await _gateway.Update(Utils.ApiConfiguration.Gateway.Services.ReleaseVersion, model, await customAuthenticationStateProvider.GetToken());
}