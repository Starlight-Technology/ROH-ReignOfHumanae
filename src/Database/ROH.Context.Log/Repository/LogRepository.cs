//-----------------------------------------------------------------------
// <copyright file="LogRepository.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.Context.Log.Interface;

namespace ROH.Context.Log.Repository;

public class LogRepository(ILogContext context) : ILogRepository
{
    public async Task SaveLogAsync(Entities.Log log, CancellationToken cancellationToken = default)
    {
        _ = await context.Logs.AddAsync(log, cancellationToken).ConfigureAwait(true);
        _ = await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
    }
}