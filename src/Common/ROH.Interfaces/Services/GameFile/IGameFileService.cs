//-----------------------------------------------------------------------
// <copyright file="IGameFileService.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.StandardModels.Response;

namespace ROH.Interfaces.Services.GameFile;

public interface IGameFileService
{
    Task<DefaultResponse> DownloadFile(Guid fileGuid);

    Task<DefaultResponse> DownloadFile(long id);

    Task SaveFileAsync(Domain.GameFiles.GameFile file, byte[] content);
}