using ROH.Blazor.Server.Interfaces.Api;
using ROH.StandardModels.Response;
using ROH.StandardModels.Version;

namespace ROH.Blazor.Server.Api
{
    public class VersionFileService : IVersionFileService
    {
        private readonly Utils.ApiConfiguration.Gateway _gateway = new();

        public async Task<DefaultResponse?> UploadVersionFile(GameVersionFileModel model) => await _gateway.Post(Utils.ApiConfiguration.Gateway.Services.UploadFile, model);

        public async Task<DefaultResponse?> GetAllVersionFiles(string versionGuid) => await _gateway.Get(Utils.ApiConfiguration.Gateway.Services.UploadFile, new { VersionGuid = versionGuid });
    }
}