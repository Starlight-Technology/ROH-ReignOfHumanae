//-----------------------------------------------------------------------
// <copyright file="IVersionFileService.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.StandardModels.Response;
using ROH.StandardModels.Version;

namespace ROH.Site.Interfaces.Api;

public interface IVersionFileService
{
    Task<DefaultResponse?> DownloadVersionFile(string FileGuid);

    Task<DefaultResponse?> GetAllVersionFiles(string VersionGuid);

    Task<DefaultResponse?> UploadVersionFile(GameVersionFileModel Model);
}