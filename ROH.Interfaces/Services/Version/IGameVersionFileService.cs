using ROH.Domain.Version;
using ROH.Models.Response;

namespace ROH.Interfaces.Services.Version
{
    public interface IGameVersionFileService
    {
        Task<DefaultResponse> DownloadFile(long id);
        Task<DefaultResponse> GetFiles(GameVersion version);
        Task NewFile(GameVersionFile file);
    }
}