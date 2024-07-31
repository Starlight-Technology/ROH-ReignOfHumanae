using ROH.StandardModels.Response;

namespace ROH.Interfaces.Services.GameFile;
public interface IGameFileService
{
    Task<DefaultResponse> DownloadFile(Guid fileGuid);
    Task<DefaultResponse> DownloadFile(long id);
    Task SaveFileAsync(Domain.GameFiles.GameFile file, byte[] content);
}