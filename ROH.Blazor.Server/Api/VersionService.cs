// Ignore Spelling: Blazor

using ROH.StandardModels.Response;
using ROH.StandardModels.Version;

namespace ROH.Blazor.Server.Api
{
    public class VersionService
    {
        private readonly Utils.ApiConfiguration.Gateway _gateway = new();

        public async Task<DefaultResponse?> GetCurrentVersion() => await _gateway.Get(Utils.ApiConfiguration.Gateway.Services.GetCurrentVersion, new List<Utils.ApiConfiguration.ApiParameters>());

        public async Task<DefaultResponse?> CreateNewVersion(GameVersionModel model) => await _gateway.Post(Utils.ApiConfiguration.Gateway.Services.CreateNewVersion, model);
    }
}