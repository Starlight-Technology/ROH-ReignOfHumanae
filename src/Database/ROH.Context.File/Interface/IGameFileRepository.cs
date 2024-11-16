//-----------------------------------------------------------------------
// <copyright file="IGameFileRepository.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using ROH.Context.File.Entities;

namespace ROH.Context.File.Interface;

public interface IGameFileRepository
{
    Task<GameFile?> GetFileAsync(Guid fileGuid, CancellationToken cancellationToken = default);

    ValueTask<GameFile?> GetFileAsync(long id, CancellationToken cancellationToken = default);

    Task SaveFileAsync(GameFile file, CancellationToken cancellationToken = default);

    Task UpdateFileAsync(GameFile file, CancellationToken cancellationToken = default);
}