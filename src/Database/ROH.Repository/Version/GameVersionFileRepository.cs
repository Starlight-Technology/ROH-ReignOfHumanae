//-----------------------------------------------------------------------
// <copyright file="GameVersionFileRepository.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;

using ROH.Domain.Version;
using ROH.Interfaces;
using ROH.Interfaces.Repository.Version;

using System.Threading;

namespace ROH.Repository.Version;

public class GameVersionFileRepository(ISqlContext context) : IGameVersionFileRepository
{
    public async Task<GameVersionFile?> GetFile(long id, CancellationToken cancellationToken = default)
    {
        GameVersionFile? gameVersionFile = await context.GameVersionFiles.FindAsync([id, cancellationToken], cancellationToken: cancellationToken)
                                                                         .ConfigureAwait(true);
        gameVersionFile!.GameFile = await context.GameFiles.FindAsync([gameVersionFile.IdGameFile], cancellationToken: cancellationToken)
                                                           .ConfigureAwait(true);

        return gameVersionFile;
    }

    public async Task<GameVersionFile?> GetFile(Guid fileGuid, CancellationToken cancellationToken = default)
    {
        GameVersionFile? gameVersionFile = await context.GameVersionFiles.FirstOrDefaultAsync(v => v.Guid == fileGuid, cancellationToken: cancellationToken)
                                                                         .ConfigureAwait(true);
        gameVersionFile!.GameFile = await context.GameFiles.FindAsync([gameVersionFile.IdGameFile], cancellationToken: cancellationToken)
                                                           .ConfigureAwait(true);

        return gameVersionFile;
    }

    public async Task<List<GameVersionFile>> GetFiles(GameVersion version, CancellationToken cancellationToken = default)
    {
        long versionId = context.GameVersions.FirstAsync(v => v.Guid == version.Guid, cancellationToken: cancellationToken).Result.Id;

        var result = await context.GameVersionFiles.Where(v => v.IdVersion == versionId)
                                             .ToListAsync(cancellationToken: cancellationToken)
                                             .ConfigureAwait(true);

        return result;
    }

    public async Task<List<GameVersionFile>> GetFiles(Guid versionGuid, CancellationToken cancellationToken = default)
    {
        long versionId = context.GameVersions.FirstAsync(v => v.Guid == versionGuid, cancellationToken: cancellationToken).Result.Id;

        var result = await context.GameVersionFiles.Where(v => v.IdVersion == versionId)
                                             .ToListAsync(cancellationToken: cancellationToken)
                                             .ConfigureAwait(true);

        return result;
    }

    public async Task SaveFile(GameVersionFile file, CancellationToken cancellationToken = default)
    {
        file.GameVersion = await context.GameVersions.FirstAsync(v => v.Guid == file.GameVersion!.Guid, cancellationToken: cancellationToken)
                                                     .ConfigureAwait(true);

        _ = await context.GameVersionFiles.AddAsync(file, cancellationToken).ConfigureAwait(true);
        _ = await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
    }
}