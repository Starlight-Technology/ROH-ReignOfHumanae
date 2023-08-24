// Ignore Spelling: Blazor

using ROH.StandardModels.Response;
using ROH.StandardModels.Version;

namespace ROH.Blazor.Server.Api
{
    public class VersionService
    {
        private readonly Utils.ApiConfiguration.Api _api = new();

        public async Task<DefaultResponse?> GetCurrentVersion() => await _api.Get(Utils.ApiConfiguration.Api.Services.GetCurrentVersion, new List<Utils.ApiConfiguration.ApiParameters>());

        public async Task<DefaultResponse?> CreateNewVersion(GameVersionModel model) => await _api.Post(Utils.ApiConfiguration.Api.Services.CreateNewVersion, model);
    }
}
