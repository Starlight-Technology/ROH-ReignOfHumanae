//-----------------------------------------------------------------------
// <copyright file="GameVersionFileRepository.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;

using ROH.Context.File.Entities;
using ROH.Context.File.Interface;

namespace ROH.Context.File.Repository;

public class GameVersionFileRepository(IFileContext context)
: IGameVersionFileRepository
{
    public async Task<GameVersionFile?> GetFileAsync(long id, CancellationToken cancellationToken = default)
    {
        GameVersionFile? gameVersionFile = await context.GameVersionFiles.FindAsync([id, cancellationToken], cancellationToken: cancellationToken)
                                                                         .ConfigureAwait(true);
        gameVersionFile!.GameFile = await context.GameFiles.FindAsync([gameVersionFile.IdGameFile], cancellationToken: cancellationToken)
                                                           .ConfigureAwait(true);

        return gameVersionFile;
    }

    public async Task<GameVersionFile?> GetFileAsync(Guid fileGuid, CancellationToken cancellationToken = default)
    {
        GameVersionFile? gameVersionFile = await context.GameVersionFiles.FirstOrDefaultAsync(v => v.Guid == fileGuid, cancellationToken: cancellationToken)
                                                                         .ConfigureAwait(true);
        gameVersionFile!.GameFile = await context.GameFiles.FindAsync([gameVersionFile.IdGameFile], cancellationToken: cancellationToken)
                                                           .ConfigureAwait(true);

        return gameVersionFile;
    }

    public async Task<List<GameVersionFile>> GetFilesAsync(long versionId, CancellationToken cancellationToken = default)
    {
        var result = await context.GameVersionFiles.Where(v => v.IdVersion == versionId)
                                             .ToListAsync(cancellationToken: cancellationToken)
                                             .ConfigureAwait(true);

        return result;
    }

    public async Task SaveFileAsync(GameVersionFile file, CancellationToken cancellationToken = default)
    {
        _ = await context.GameVersionFiles.AddAsync(file, cancellationToken).ConfigureAwait(true);
        _ = await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
    }
}