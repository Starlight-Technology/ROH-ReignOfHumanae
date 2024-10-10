//-----------------------------------------------------------------------
// <copyright file="LogRepository.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.Interfaces;
using ROH.Interfaces.Repository.Log;

namespace ROH.Repository.Log;

public class LogRepository(ISqlContext context) : ILogRepository
{
    public async Task SaveLog(Domain.Logging.Log log, CancellationToken cancellationToken = default)
    {
        _ = await context.Logs.AddAsync(log, cancellationToken).ConfigureAwait(true);
        _ = await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
    }
}