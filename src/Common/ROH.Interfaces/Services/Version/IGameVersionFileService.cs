//-----------------------------------------------------------------------
// <copyright file="IGameVersionFileService.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.StandardModels.Response;
using ROH.StandardModels.Version;

namespace ROH.Interfaces.Services.Version;

public interface IGameVersionFileService
{
    Task<DefaultResponse> DownloadFile(long id);

    Task<DefaultResponse> DownloadFile(Guid fileGuid);

    Task<DefaultResponse> GetFiles(string versionGuid);

    Task<DefaultResponse> NewFile(GameVersionFileModel fileModel);
}