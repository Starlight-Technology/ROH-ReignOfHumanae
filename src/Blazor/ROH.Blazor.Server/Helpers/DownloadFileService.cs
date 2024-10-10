//-----------------------------------------------------------------------
// <copyright file="DownloadFileService.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.JSInterop;

using ROH.Blazor.Server.Interfaces.Helpers;
using ROH.StandardModels.File;

namespace ROH.Blazor.Server.Helpers;

public class DownloadFileService(IJSRuntime _jsRuntime) : IDownloadFileService
{
    public async Task Download(GameFileModel fileModel) => await _jsRuntime.InvokeVoidAsync(
        "window.DownloadFile",
        fileModel.Name,
        fileModel.Content);
}
