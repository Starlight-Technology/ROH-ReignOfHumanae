using Microsoft.JSInterop;

using ROH.Blazor.Server.Interfaces.Helpers;
using ROH.StandardModels.File;

namespace ROH.Blazor.Server.Helpers;

public class DownloadFileService(IJSRuntime _jsRuntime) : IDownloadFileService
{
    public async Task Download(FileModel fileModel) => await _jsRuntime.InvokeVoidAsync("window.DownloadFile", fileModel.Name, fileModel.Content);
}
