// Ignore Spelling: Blazor

using ROH.StandardModels.Response;
using ROH.StandardModels.Version;

namespace ROH.Blazor.Server.Interfaces.Api
{
    public interface IVersionService
    {
        Task<DefaultResponse?> CreateNewVersion(GameVersionModel model);

        Task<DefaultResponse?> GetAllReleasedVersionsPaginated(int page = 1, int take = 10);

        Task<DefaultResponse?> GetAllVersionsPaginated(int page = 1, int take = 10);

        Task<DefaultResponse?> GetCurrentVersion();

        Task<DefaultResponse?> GetVersionDetails(Guid guid);

        Task<DefaultResponse?> ReleaseVersion(GameVersionModel model);
    }
}