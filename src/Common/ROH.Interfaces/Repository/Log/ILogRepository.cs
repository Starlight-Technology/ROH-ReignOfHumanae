//-----------------------------------------------------------------------
// <copyright file="ILogRepository.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ROH.Interfaces.Repository.Log;

public interface ILogRepository
{
    Task SaveLog(Domain.Logging.Log log, CancellationToken cancellationToken = default);
}