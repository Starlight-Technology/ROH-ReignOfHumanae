//-----------------------------------------------------------------------
// <copyright file="IGameVersionService.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.StandardModels.Response;
using ROH.StandardModels.Version;

namespace ROH.Service.Version.Interface;

public interface IGameVersionService
{
    Task<DefaultResponse> GetAllReleasedVersionsAsync(
        int take = 10,
        int page = 1,
        CancellationToken cancellationToken = default);

    Task<DefaultResponse> GetAllVersionsAsync(
        int take = 10,
        int page = 1,
        CancellationToken cancellationToken = default);

    Task<DefaultResponse> GetCurrentVersionAsync(CancellationToken cancellationToken = default);

    Task<DefaultResponse> GetVersionByGuidAsync(string versionGuid, CancellationToken cancellationToken = default);

    Task<DefaultResponse> NewVersionAsync(GameVersionModel version, CancellationToken cancellationToken = default);

    Task<DefaultResponse> SetReleasedAsync(string versionGuid, CancellationToken cancellationToken = default);

    Task<bool> VerifyIfVersionExistAsync(GameVersionModel version, CancellationToken cancellationToken = default);

    Task<bool> VerifyIfVersionExistAsync(string versionGuid, CancellationToken cancellationToken = default);
}