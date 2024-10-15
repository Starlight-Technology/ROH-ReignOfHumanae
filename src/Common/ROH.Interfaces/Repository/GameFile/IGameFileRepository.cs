//-----------------------------------------------------------------------
// <copyright file="IGameFileRepository.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ROH.Interfaces.Repository.GameFile;

public interface IGameFileRepository
{
    Task<Domain.GameFiles.GameFile?> GetFileAsync(Guid fileGuid, CancellationToken cancellationToken = default);

    ValueTask<Domain.GameFiles.GameFile?> GetFileAsync(long id, CancellationToken cancellationToken = default);

    Task SaveFileAsync(Domain.GameFiles.GameFile file, CancellationToken cancellationToken = default);

    Task UpdateFileAsync(Domain.GameFiles.GameFile file, CancellationToken cancellationToken = default);
}