//-----------------------------------------------------------------------
// <copyright file="GameFileRepository.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;

using ROH.Context.File.Entities;
using ROH.Context.File.Interface;

namespace ROH.Context.File.Repository;

public class GameFileRepository(IFileContext context) : IGameFileRepository
{
    public ValueTask<GameFile?> GetFileAsync(long id, CancellationToken cancellationToken = default) => context.GameFiles.FindAsync([id, cancellationToken], cancellationToken: cancellationToken);

    public Task<GameFile?> GetFileAsync(Guid fileGuid, CancellationToken cancellationToken = default)
        => context.GameFiles.FirstOrDefaultAsync(v => v.Guid == fileGuid, cancellationToken);

    public async Task SaveFileAsync(GameFile file, CancellationToken cancellationToken = default)
    {
        await context.GameFiles.AddAsync(file, cancellationToken).ConfigureAwait(true);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
    }

    public Task UpdateFileAsync(GameFile file, CancellationToken cancellationToken = default)
    {
        context.GameFiles.Update(file);
        context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

        return Task.CompletedTask;
    }
}
