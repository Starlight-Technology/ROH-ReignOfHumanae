// Ignore Spelling: Blazor

using ROH.Blazor.Server.Interfaces.Api;
using ROH.StandardModels.Response;
using ROH.StandardModels.Version;

namespace ROH.Blazor.Server.Api
{
    public class VersionService : IVersionService
    {
        private readonly Utils.ApiConfiguration.Gateway _gateway = new();

        public async Task<DefaultResponse?> GetCurrentVersion() => await _gateway.Get(Utils.ApiConfiguration.Gateway.Services.GetCurrentVersion, new List<Utils.ApiConfiguration.ApiParameters>());

        public async Task<DefaultResponse?> CreateNewVersion(GameVersionModel model) => await _gateway.Post(Utils.ApiConfiguration.Gateway.Services.CreateNewVersion, model);

        public async Task<DefaultResponse?> GetAllVersionsPaginated(int page = 1, int take = 10) => await _gateway.Get(Utils.ApiConfiguration.Gateway.Services.GetAllVersionsPaginated, new List<Utils.ApiConfiguration.ApiParameters>
        {
            new Utils.ApiConfiguration.ApiParameters() {Name="page", Value= page.ToString() },
            new Utils.ApiConfiguration.ApiParameters() {Name="take", Value= take.ToString() },
        });

        public async Task<DefaultResponse?> GetAllReleasedVersionsPaginated(int page = 1, int take = 10) => await _gateway.Get(Utils.ApiConfiguration.Gateway.Services.GetAllReleasedVersionsPaginated, new List<Utils.ApiConfiguration.ApiParameters>
        {
            new Utils.ApiConfiguration.ApiParameters() {Name="page", Value= page.ToString() },
            new Utils.ApiConfiguration.ApiParameters() {Name="take", Value= take.ToString() },
        });

        public async Task<DefaultResponse?> GetVersionDetails(Guid guid) => await _gateway.Get(Utils.ApiConfiguration.Gateway.Services.GetVersionDetails, new List<Utils.ApiConfiguration.ApiParameters>
        {
            new Utils.ApiConfiguration.ApiParameters() {Name="guid", Value= guid.ToString() },
        });
    }
}