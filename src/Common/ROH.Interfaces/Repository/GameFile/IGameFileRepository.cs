namespace ROH.Interfaces.Repository.GameFile;

public interface IGameFileRepository
{
    Task<Domain.GameFiles.GameFile?> GetFileAsync(Guid fileGuid);

    Task<Domain.GameFiles.GameFile?> GetFileAsync(long id);

    Task SaveFileAsync(Domain.GameFiles.GameFile file);

    Task UpdateFileAsync(Domain.GameFiles.GameFile file);
}