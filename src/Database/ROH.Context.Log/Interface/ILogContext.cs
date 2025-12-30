//-----------------------------------------------------------------------
// <copyright file="ILogContext.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;

namespace ROH.Context.Log.Interface;

public interface ILogContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    DbSet<Entities.Log> Logs { get; }
}