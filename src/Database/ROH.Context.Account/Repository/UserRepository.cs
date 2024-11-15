//-----------------------------------------------------------------------
// <copyright file="UserRepository.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;

using ROH.Context.Account.Entity;
using ROH.Context.Account.Interface;

namespace ROH.Context.Account.Repository;

public class UserRepository(IAccountContext context) : IUserRepository
{
    public async Task<User> CreateNewUserAsync(User user, CancellationToken cancellationToken = default)
    {
        await context.Users.AddAsync(user, cancellationToken).ConfigureAwait(true);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
        return user;
    }

    public Task<bool> EmailInUseAsync(string email, CancellationToken cancellationToken = default)
        => context.Users.AnyAsync(u => string.Equals(u.Email, email), cancellationToken);

    public Task<User?> FindUserByEmailAsync(string email, CancellationToken cancellationToken = default)
        => context.Users.FirstOrDefaultAsync(u => string.Equals(u.Email, email), cancellationToken);

    public Task<User?> FindUserByUserNameAsync(string userName, CancellationToken cancellationToken = default)
        => context.Users.FirstOrDefaultAsync(u => string.Equals(u.UserName, userName), cancellationToken);

    public Task<User> GetUserByGuidAsync(Guid userGuid, CancellationToken cancellationToken = default)
        => context.Users.FirstAsync(u => u.Guid == userGuid, cancellationToken);

}
