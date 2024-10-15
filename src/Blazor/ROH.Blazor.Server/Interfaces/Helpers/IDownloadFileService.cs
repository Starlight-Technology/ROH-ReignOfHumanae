//-----------------------------------------------------------------------
// <copyright file="IDownloadFileService.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.StandardModels.File;

namespace ROH.Blazor.Server.Interfaces.Helpers;

public interface IDownloadFileService
{
    Task Download(GameFileModel fileModel);
}