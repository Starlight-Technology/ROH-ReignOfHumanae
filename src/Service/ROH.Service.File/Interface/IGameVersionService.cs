//-----------------------------------------------------------------------
// <copyright file="IGameVersionService.cs" company="Starlight-Technology">
//     Author:
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ROH.Service.File.Interface;

public interface IGameVersionService
{
    Task<VersionServiceApi.DefaultResponse> GetCurrentVersionAsync(CancellationToken cancellationToken = default);

    Task<bool> VerifyIfVersionExistAsync(Guid versionGuid, CancellationToken cancellationToken = default);
}