//-----------------------------------------------------------------------
// <copyright file="IGameFileService.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.Context.File.Entities;
using ROH.StandardModels.Response;

namespace ROH.Service.File.Interface;

public interface IGameFileService
{
    Task<DefaultResponse> DownloadFileAsync(Guid fileGuid, CancellationToken cancellationToken = default);

    Task<DefaultResponse> DownloadFileAsync(long id, CancellationToken cancellationToken = default);

    Task SaveFileAsync(GameFile file, byte[] content, CancellationToken cancellationToken = default);
}