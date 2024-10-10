//-----------------------------------------------------------------------
// <copyright file="IGameVersionService.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.StandardModels.Response;
using ROH.StandardModels.Version;

namespace ROH.Interfaces.Services.Version;

public interface IGameVersionService
{
    Task<DefaultResponse> GetAllReleasedVersions(int take = 10, int page = 1);

    Task<DefaultResponse> GetAllVersions(int take = 10, int page = 1);

    Task<DefaultResponse> GetCurrentVersion();

    Task<DefaultResponse> GetVersionByGuid(string versionGuid);

    Task<DefaultResponse> NewVersion(GameVersionModel version);

    Task<DefaultResponse> SetReleased(string versionGuid);

    Task<bool> VerifyIfVersionExist(GameVersionModel version);

    Task<bool> VerifyIfVersionExist(string versionGuid);
}