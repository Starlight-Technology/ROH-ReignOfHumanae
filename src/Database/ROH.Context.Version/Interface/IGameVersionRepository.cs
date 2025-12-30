//-----------------------------------------------------------------------
// <copyright file="GameVersionRepository.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.Context.Version.Entities;
using ROH.Context.Version.Paginator;

namespace ROH.Context.Version.Interface;

public interface IGameVersionRepository
{
    Task<Paginated> GetAllReleasedVersionsAsync(
        int take = 10,
        int skip = 0,
        CancellationToken cancellationToken = default);

    Task<Paginated> GetAllVersionsAsync(int take = 10, int skip = 0, CancellationToken cancellationToken = default);

    Task<GameVersion?> GetCurrentGameVersionAsync(CancellationToken cancellationToken = default);

    Task<GameVersion?> GetVersionByGuidAsync(Guid versionGuid, CancellationToken cancellationToken = default);

    Task<GameVersion> SetNewGameVersionAsync(GameVersion version, CancellationToken cancellationToken = default);

    Task<GameVersion> UpdateGameVersionAsync(GameVersion version, CancellationToken cancellationToken = default);

    Task<bool> VerifyIfExistAsync(GameVersion version, CancellationToken cancellationToken = default);

    Task<bool> VerifyIfExistAsync(Guid versionGuid, CancellationToken cancellationToken = default);
}