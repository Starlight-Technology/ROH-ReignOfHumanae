//-----------------------------------------------------------------------
// <copyright file="AccountRepository.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;

using ROH.Context.Account.Entity;
using ROH.Context.Account.Interface;

namespace ROH.Context.Account.Repository;

public class AccountRepository(IAccountContext context)
{
    public Task<Entity.Account?> GetAccountByGuidAsync(Guid guid, CancellationToken cancellationToken = default) => context.Accounts
        .AsNoTracking()
        .FirstOrDefaultAsync(a => a.Guid == guid, cancellationToken);

    public ValueTask<Entity.Account?> GetAccountByIdAsync(long id, CancellationToken cancellationToken = default) => context.Accounts.FindAsync([id, cancellationToken], cancellationToken: cancellationToken);

    public async Task<Entity.Account?> GetAccountByUserGuidAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        User? user = await context.Users.AsNoTracking().FirstOrDefaultAsync(a => a.Guid == guid, cancellationToken).ConfigureAwait(true);

        return user is null ? null : await context.Accounts.AsNoTracking().FirstAsync(a => a.Id == user.IdAccount, cancellationToken).ConfigureAwait(true);
    }

    public async Task UpdateAccountAsync(Entity.Account account, CancellationToken cancellationToken = default)
    {
        _ = context.Accounts.Update(account);
        _ = await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
    }
}
