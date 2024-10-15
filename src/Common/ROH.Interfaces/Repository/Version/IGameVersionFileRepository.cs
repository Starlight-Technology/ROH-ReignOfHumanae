//-----------------------------------------------------------------------
// <copyright file="IGameVersionFileRepository.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.Domain.Version;

namespace ROH.Interfaces.Repository.Version;

public interface IGameVersionFileRepository
{
    Task<GameVersionFile?> GetFileAsync(long id, CancellationToken cancellationToken = default);

    Task<GameVersionFile?> GetFileAsync(Guid fileGuid, CancellationToken cancellationToken = default);

    Task<List<GameVersionFile>> GetFilesAsync(GameVersion version, CancellationToken cancellationToken = default);

    Task<List<GameVersionFile>> GetFilesAsync(Guid versionGuid, CancellationToken cancellationToken = default);

    Task SaveFileAsync(GameVersionFile file, CancellationToken cancellationToken = default);
}