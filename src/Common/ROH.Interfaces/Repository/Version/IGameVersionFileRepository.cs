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
    Task<GameVersionFile?> GetFile(long id, CancellationToken cancellationToken = default);

    Task<GameVersionFile?> GetFile(Guid fileGuid, CancellationToken cancellationToken = default);

    Task<List<GameVersionFile>> GetFiles(GameVersion version, CancellationToken cancellationToken = default);

    Task<List<GameVersionFile>> GetFiles(Guid versionGuid, CancellationToken cancellationToken = default);

    Task SaveFile(GameVersionFile file, CancellationToken cancellationToken = default);
}