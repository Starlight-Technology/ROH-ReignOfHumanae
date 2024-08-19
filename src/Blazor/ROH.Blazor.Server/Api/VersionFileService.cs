using ROH.Blazor.Server.Interfaces.Api;
using ROH.Blazor.Server.Interfaces.Helpers;
using ROH.StandardModels.Response;
using ROH.StandardModels.Version;

namespace ROH.Blazor.Server.Api;

public class VersionFileService(ICustomAuthenticationStateProvider customAuthenticationStateProvider) : IVersionFileService
{
    private readonly Utils.ApiConfiguration.Gateway _gateway = new();

    public async Task<DefaultResponse?> UploadVersionFile(GameVersionFileModel Model) => await _gateway.Post(Utils.ApiConfiguration.Gateway.Services.UploadFile, Model, await customAuthenticationStateProvider.GetToken());

    public async Task<DefaultResponse?> GetAllVersionFiles(string VersionGuid) => await _gateway.Get(Utils.ApiConfiguration.Gateway.Services.GetAllVersionFiles, new { VersionGuid }, await customAuthenticationStateProvider.GetToken());

    public async Task<DefaultResponse?> DownloadVersionFile(string FileGuid) => await _gateway.Get(Utils.ApiConfiguration.Gateway.Services.DownloadFile, new { FileGuid }, await customAuthenticationStateProvider.GetToken());
}