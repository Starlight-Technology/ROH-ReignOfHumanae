using ROH.StandardModels.Response;
using ROH.StandardModels.Version;

namespace ROH.Interfaces.Services.Version
{
    public interface IGameVersionFileService
    {
        Task<DefaultResponse> DownloadFile(long id);

        Task<DefaultResponse> GetFiles(GameVersionModel version);

        Task<DefaultResponse> NewFile(GameVersionFileModel file);
    }
}