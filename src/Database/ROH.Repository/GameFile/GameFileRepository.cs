//-----------------------------------------------------------------------
// <copyright file="GameFileRepository.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;

using ROH.Interfaces;
using ROH.Interfaces.Repository.GameFile;

namespace ROH.Repository.GameFile;

public class GameFileRepository(ISqlContext context) : IGameFileRepository
{
    public ValueTask<Domain.GameFiles.GameFile?> GetFileAsync(long id, CancellationToken cancellationToken = default) => context.GameFiles.FindAsync([id, cancellationToken], cancellationToken: cancellationToken);

    public Task<Domain.GameFiles.GameFile?> GetFileAsync(Guid fileGuid, CancellationToken cancellationToken = default)
        => context.GameFiles.FirstOrDefaultAsync(v => v.Guid == fileGuid, cancellationToken);

    public async Task SaveFileAsync(Domain.GameFiles.GameFile file, CancellationToken cancellationToken = default)
    {
        await context.GameFiles.AddAsync(file, cancellationToken).ConfigureAwait(true);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
    }

    public Task UpdateFileAsync(Domain.GameFiles.GameFile file, CancellationToken cancellationToken = default)
    {
        context.GameFiles.Update(file);
        context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);

        return Task.CompletedTask;
    }
}
