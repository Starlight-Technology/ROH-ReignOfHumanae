//-----------------------------------------------------------------------
// <copyright file="IAccountContext.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;

using ROH.Context.Account.Entity;

namespace ROH.Context.Account.Interface;

public interface IAccountContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    DbSet<Entity.Account> Accounts { get; set; }

    DbSet<User> Users { get; set; }
}