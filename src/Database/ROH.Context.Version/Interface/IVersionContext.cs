//-----------------------------------------------------------------------
// <copyright file="IVersionContext.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;

using ROH.Context.Version.Entities;

namespace ROH.Context.Version.Interface;

public interface IVersionContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    DbSet<GameVersion> GameVersions { get; set; }
}