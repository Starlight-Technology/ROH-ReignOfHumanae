namespace ROH.Interfaces.Repository.GameFile;

public interface IGameFileRepository
{
    Task<Domain.GameFiles.GameFile?> GetFile(Guid fileGuid);

    Task<Domain.GameFiles.GameFile?> GetFile(long id);

    Task SaveFile(Domain.GameFiles.GameFile file);
}