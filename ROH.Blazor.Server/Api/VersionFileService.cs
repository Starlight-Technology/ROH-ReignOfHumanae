using ROH.Blazor.Server.Interfaces.Api;
using ROH.StandardModels.Response;
using ROH.StandardModels.Version;
using ROH.Utils.ApiConfiguration;

namespace ROH.Blazor.Server.Api
{
    public class VersionFileService : IVersionFileService
    {
        private readonly Utils.ApiConfiguration.Gateway _gateway = new();

        public async Task<DefaultResponse?> UploadVersionFile(GameVersionFileModel model)
        {
            return await _gateway.Post(Utils.ApiConfiguration.Gateway.Services.UploadFile, model);
        }

        public async Task<DefaultResponse?> GetAllVersionFiles(string versionGuid)
        {
            return await _gateway.Get(Utils.ApiConfiguration.Gateway.Services.UploadFile, [new ApiParameters() { Name = "versionGuid", Value = versionGuid }]);
        }
    }
}