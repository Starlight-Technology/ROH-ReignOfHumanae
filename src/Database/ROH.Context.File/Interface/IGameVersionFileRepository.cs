//-----------------------------------------------------------------------
// <copyright file="GameVersionFileRepository.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

//-----------------------------------------------------------------------
// <copyright file="GameVersionFileRepository.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.Context.File.Entities;

namespace ROH.Context.File.Interface;

public interface IGameVersionFileRepository

{
    Task<GameVersionFile?> GetFileAsync(Guid fileGuid, CancellationToken cancellationToken = default);

    Task<GameVersionFile?> GetFileAsync(long id, CancellationToken cancellationToken = default);

    Task<List<GameVersionFile>> GetFilesAsync(Guid versionGuid, CancellationToken cancellationToken = default);

    Task SaveFileAsync(GameVersionFile file, CancellationToken cancellationToken = default);
}