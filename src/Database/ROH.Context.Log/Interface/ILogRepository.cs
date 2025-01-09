//-----------------------------------------------------------------------
// <copyright file="ILogRepository.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ROH.Context.Log.Interface;

public interface ILogRepository
{
    Task SaveLogAsync(Entities.Log log, CancellationToken cancellationToken = default);
}