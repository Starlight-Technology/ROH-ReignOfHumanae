// Ignore Spelling: Blazor

using ROH.Blazor.Server.Interfaces.Api;
using ROH.StandardModels.Response;
using ROH.StandardModels.Version;

namespace ROH.Blazor.Server.Api
{
    public class VersionService : IVersionService
    {
        private readonly Utils.ApiConfiguration.Gateway _gateway = new();

        public async Task<DefaultResponse?> GetCurrentVersion()
        {
            return await _gateway.Get(Utils.ApiConfiguration.Gateway.Services.GetCurrentVersion, []);
        }

        public async Task<DefaultResponse?> CreateNewVersion(GameVersionModel model)
        {
            return await _gateway.Post(Utils.ApiConfiguration.Gateway.Services.CreateNewVersion, model);
        }

        public async Task<DefaultResponse?> GetAllVersionsPaginated(int page = 1, int take = 10)
        {
            return await _gateway.Get(Utils.ApiConfiguration.Gateway.Services.GetAllVersionsPaginated,
        [
            new Utils.ApiConfiguration.ApiParameters() { Name = "page", Value = page.ToString() },
            new Utils.ApiConfiguration.ApiParameters() { Name = "take", Value = take.ToString() },
        ]);
        }

        public async Task<DefaultResponse?> GetAllReleasedVersionsPaginated(int page = 1, int take = 10)
        {
            return await _gateway.Get(Utils.ApiConfiguration.Gateway.Services.GetAllReleasedVersionsPaginated,
        [
            new Utils.ApiConfiguration.ApiParameters() { Name = "page", Value = page.ToString() },
            new Utils.ApiConfiguration.ApiParameters() { Name = "take", Value = take.ToString() },
        ]);
        }

        public async Task<DefaultResponse?> GetVersionDetails(Guid guid)
        {
            return await _gateway.Get(Utils.ApiConfiguration.Gateway.Services.GetVersionDetails,
        [
            new Utils.ApiConfiguration.ApiParameters() { Name = "guid", Value = guid.ToString() },
        ]);
        }
    }
}