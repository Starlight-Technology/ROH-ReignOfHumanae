using ROH.Blazor.Server.Interfaces.Api;
using ROH.StandardModels.Response;
using ROH.StandardModels.Version;

namespace ROH.Blazor.Server.Api
{
    public class VersionFileService : IVersionFileService
    {
        private readonly Utils.ApiConfiguration.Gateway _gateway = new();

        public async Task<DefaultResponse?> UploadVersionFile(GameVersionFileModel Model) => await _gateway.Post(Utils.ApiConfiguration.Gateway.Services.UploadFile, Model);

        public async Task<DefaultResponse?> GetAllVersionFiles(string VersionGuid) => await _gateway.Get(Utils.ApiConfiguration.Gateway.Services.UploadFile, new { VersionGuid });
    }
}