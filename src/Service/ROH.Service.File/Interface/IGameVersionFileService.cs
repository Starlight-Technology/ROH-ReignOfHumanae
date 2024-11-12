//-----------------------------------------------------------------------
// <copyright file="IGameVersionFileService.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.StandardModels.Response;
using ROH.StandardModels.Version;

namespace ROH.Service.File.Interface;

public interface IGameVersionFileService
{
    Task<DefaultResponse> DownloadFileAsync(long id, CancellationToken cancellationToken = default);

    Task<DefaultResponse> DownloadFileAsync(Guid fileGuid, CancellationToken cancellationToken = default);

    Task<DefaultResponse> GetFilesAsync(string versionGuid, CancellationToken cancellationToken = default);

    Task<DefaultResponse> NewFileAsync(GameVersionFileModel fileModel, CancellationToken cancellationToken = default);
}