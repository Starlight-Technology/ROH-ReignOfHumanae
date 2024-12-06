//-----------------------------------------------------------------------
// <copyright file="GameVersionRepository.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;

using ROH.Context.Version.Entities;
using ROH.Context.Version.Interface;
using ROH.Context.Version.Paginator;

namespace ROH.Context.Version.Repository;

public class GameVersionRepository(IVersionContext context)
: IGameVersionRepository
{
    public async Task<Paginated> GetAllReleasedVersionsAsync(int take = 10, int skip = 0, CancellationToken cancellationToken = default)
    {
        List<GameVersion> versions = await context.GameVersions
            .Where(v => v.Released)
            .OrderBy(gv => gv.Version)
            .ThenBy(gv => gv.Release)
            .ThenBy(gv => gv.Review)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(true);
        int total = versions.Count;
        return new(total, versions.Cast<dynamic>().ToList());
    }

    public async Task<Paginated> GetAllVersionsAsync(int take = 10, int skip = 0, CancellationToken cancellationToken = default)
    {
        List<GameVersion> versions = await context.GameVersions
            .OrderBy(gv => gv.Version)
            .ThenBy(gv => gv.Release)
            .ThenBy(gv => gv.Review)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(true);
        int total = await context.GameVersions.CountAsync(cancellationToken).ConfigureAwait(true);
        return new(total, versions.Cast<dynamic>().ToList());
    }

    public Task<GameVersion?> GetCurrentGameVersionAsync(CancellationToken cancellationToken = default) => context.GameVersions
        .Where(v => v.Released)
        .OrderByDescending(v => v.ReleaseDate)
        .FirstOrDefaultAsync(cancellationToken);

    public Task<GameVersion?> GetVersionByGuidAsync(Guid versionGuid, CancellationToken cancellationToken = default) => context.GameVersions
        .FirstOrDefaultAsync(v => v.Guid == versionGuid,
                             cancellationToken);

    public async Task<GameVersion> SetNewGameVersionAsync(GameVersion version, CancellationToken cancellationToken = default)
    {
        version = version with { VersionDate = DateTime.UtcNow };
        _ = await context.GameVersions.AddAsync(version, cancellationToken).ConfigureAwait(true);
        _ = await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

        return version;
    }

    public async Task<GameVersion> UpdateGameVersionAsync(GameVersion version, CancellationToken cancellationToken = default)
    {
        _ = context.GameVersions.Update(version);
        _ = await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

        return version;
    }

    public Task<bool> VerifyIfExistAsync(GameVersion version, CancellationToken cancellationToken = default) => context.GameVersions
        .AnyAsync(v => v.Release == version.Release && v.Review == version.Review && v.Version == version.Version, cancellationToken);

    public Task<bool> VerifyIfExistAsync(Guid versionGuid, CancellationToken cancellationToken = default) => context.GameVersions
        .AnyAsync(v => v.Guid == versionGuid, cancellationToken);
}