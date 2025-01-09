namespace ROH.Service.File.Interface;

public interface IGameVersionService
{
    Task<VersionServiceApi.DefaultResponse> GetCurrentVersionAsync(CancellationToken cancellationToken = default);

    Task<bool> VerifyIfVersionExistAsync(
        Guid versionGuid,
        CancellationToken cancellationToken = default);
}
