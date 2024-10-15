//-----------------------------------------------------------------------
// <copyright file="VersionFileService.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.Blazor.Server.Interfaces.Api;
using ROH.Blazor.Server.Interfaces.Helpers;
using ROH.StandardModels.Response;
using ROH.StandardModels.Version;
using ROH.Utils.ApiConfiguration;

namespace ROH.Blazor.Server.Api;

public class VersionFileService(ICustomAuthenticationStateProvider customAuthenticationStateProvider) : IVersionFileService
{
    private readonly Gateway _gateway = new();

    public async Task<DefaultResponse?> DownloadVersionFile(string FileGuid) => await _gateway.GetAsync(
        Gateway.Services.DownloadFile,
        new { FileGuid },
        await customAuthenticationStateProvider.GetToken());

    public async Task<DefaultResponse?> GetAllVersionFiles(string VersionGuid) => await _gateway.GetAsync(
        Gateway.Services.GetAllVersionFiles,
        new { VersionGuid },
        await customAuthenticationStateProvider.GetToken());

    public async Task<DefaultResponse?> UploadVersionFile(GameVersionFileModel Model) => await _gateway.PostAsync(
        Gateway.Services.UploadFile,
        Model,
        await customAuthenticationStateProvider.GetToken());
}