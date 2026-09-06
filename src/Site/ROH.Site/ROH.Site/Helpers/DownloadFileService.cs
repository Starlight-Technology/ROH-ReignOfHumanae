//-----------------------------------------------------------------------
// <copyright file="DownloadFileService.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.JSInterop;

using ROH.Site.Interfaces.Helpers;
using ROH.StandardModels.File;

namespace ROH.Site.Helpers;

public class DownloadFileService(IJSRuntime _jsRuntime) : IDownloadFileService
{
    public async Task Download(GameFileModel fileModel)
    {
        string fileName = Path.GetFileName(
            fileModel.Name
                .Replace('\\', Path.DirectorySeparatorChar)
                .Replace('/', Path.DirectorySeparatorChar));

        await _jsRuntime.InvokeVoidAsync(
            "window.DownloadFile",
            string.IsNullOrWhiteSpace(fileName) ? fileModel.Name : fileName,
            fileModel.Content);
    }
}
